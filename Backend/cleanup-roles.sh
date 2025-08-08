#!/bin/bash

# Script para limpiar roles y asignar nuevos roles a usuarios existentes
# Ejecutar este script en el VPS después del deploy

set -e

echo "🔧 Iniciando limpieza selectiva y reasignación de roles..."

# ========================================
# CARGAR CONFIGURACIÓN DE ROLES
# ========================================
if [ -f "role-config.sh" ]; then
    echo "📋 Cargando configuración desde role-config.sh..."
    source role-config.sh
else
    echo "⚠️  Archivo role-config.sh no encontrado, usando configuración por defecto..."
    # Configuración por defecto
    SUPERADMIN_EMAILS=("admin@fms.local")  # Cambiar por tu email
    ADMIN_EMAILS=()                        # Vacío por defecto
fi

echo "📋 Configuración de roles:"
echo "   SuperAdmins: ${SUPERADMIN_EMAILS[*]}"
echo "   Admins: ${ADMIN_EMAILS[*]}"
echo "   Otros usuarios: user (automático)"
echo ""

# Verificar que docker-compose esté disponible
if ! command -v docker-compose &> /dev/null; then
    echo "❌ docker-compose no encontrado"
    exit 1
fi

echo "📥 Descargando nueva imagen..."
docker-compose -f docker-compose.deploy.yml pull fms-api

echo "⏸️  Deteniendo aplicación (manteniendo base de datos)..."
docker-compose -f docker-compose.deploy.yml stop fms-api

echo "🧹 Limpiando roles antiguos..."
docker exec -i fms-db psql -U laubertarelli -d fms << 'EOSQL'
-- Mostrar estado actual
\echo '📊 Estado actual de usuarios y roles:'
SELECT u."UserName", u."Email", r."Name" as "Role"
FROM "AspNetUsers" u 
LEFT JOIN "AspNetUserRoles" ur ON u."Id" = ur."UserId"
LEFT JOIN "AspNetRoles" r ON ur."RoleId" = r."Id"
ORDER BY u."UserName";

-- Limpiar asignaciones de roles
\echo '🧹 Eliminando asignaciones de roles...'
DELETE FROM "AspNetUserRoles";

-- Limpiar roles antiguos
\echo '🧹 Eliminando roles antiguos...'
DELETE FROM "AspNetRoles";

\echo '✅ Limpieza completada'
EOSQL

echo "🚀 Reiniciando aplicación con roles corregidos..."
docker-compose -f docker-compose.deploy.yml up -d fms-api

echo "⏳ Esperando que la aplicación inicie y aplique migraciones..."
sleep 15

echo "� Aplicando roles a usuarios existentes..."

# Función para asignar rol a usuario por email
assign_role_by_email() {
    local email="$1"
    local role="$2"
    
    docker exec -i fms-db psql -U laubertarelli -d fms << EOSQL
DO \$\$
DECLARE
    user_id text;
    role_id text;
BEGIN
    -- Obtener ID del usuario por email
    SELECT "Id" INTO user_id FROM "AspNetUsers" WHERE "Email" = '$email';
    
    -- Obtener ID del rol
    SELECT "Id" INTO role_id FROM "AspNetRoles" WHERE "Name" = '$role';
    
    -- Si ambos existen, asignar el rol
    IF user_id IS NOT NULL AND role_id IS NOT NULL THEN
        INSERT INTO "AspNetUserRoles" ("UserId", "RoleId") 
        VALUES (user_id, role_id)
        ON CONFLICT DO NOTHING;
        RAISE NOTICE 'Rol % asignado a usuario %', '$role', '$email';
    ELSE
        RAISE NOTICE 'No se pudo asignar rol % a % (usuario o rol no encontrado)', '$role', '$email';
    END IF;
END
\$\$;
EOSQL
}

# Asignar SuperAdmin roles
for email in "${SUPERADMIN_EMAILS[@]}"; do
    echo "� Asignando SuperAdmin a: $email"
    assign_role_by_email "$email" "superadmin"
done

# Asignar Admin roles
for email in "${ADMIN_EMAILS[@]}"; do
    echo "🛡️  Asignando Admin a: $email"
    assign_role_by_email "$email" "admin"
done

}

# PRIMERO: Asignar rol 'user' a TODOS los usuarios (base obligatoria)
echo "👤 Asignando rol 'user' a TODOS los usuarios..."
docker exec -i fms-db psql -U laubertarelli -d fms << 'EOSQL'
DO $$
DECLARE
    user_record RECORD;
    user_role_id text;
BEGIN
    -- Obtener ID del rol 'user'
    SELECT "Id" INTO user_role_id FROM "AspNetRoles" WHERE "Name" = 'user';
    
    -- Para TODOS los usuarios (sin excepción)
    FOR user_record IN 
        SELECT u."Id", u."Email" 
        FROM "AspNetUsers" u 
    LOOP
        INSERT INTO "AspNetUserRoles" ("UserId", "RoleId") 
        VALUES (user_record."Id", user_role_id)
        ON CONFLICT DO NOTHING;  -- Evita duplicados si ya se asignó
        RAISE NOTICE 'Rol user asignado a: %', user_record."Email";
    END LOOP;
END
$$;
EOSQL

# SEGUNDO: Asignar roles adicionales a usuarios específicos
# Los SuperAdmins tendrán: user + superadmin
for email in "${SUPERADMIN_EMAILS[@]}"; do
    echo "👑 Asignando rol SuperAdmin adicional a: $email"
    assign_role_by_email "$email" "superadmin"
done

# Los Admins tendrán: user + admin  
for email in "${ADMIN_EMAILS[@]}"; do
    echo "🛡️  Asignando rol Admin adicional a: $email"
    assign_role_by_email "$email" "admin"
done

echo "📊 Verificando asignación final de roles..."
docker exec -i fms-db psql -U laubertarelli -d fms << 'EOSQL'
\echo '📊 Estado final de usuarios y roles:'
SELECT 
    u."UserName",
    u."Email", 
    r."Name" as "Role",
    r."Id" as "RoleId"
FROM "AspNetUsers" u 
JOIN "AspNetUserRoles" ur ON u."Id" = ur."UserId"
JOIN "AspNetRoles" r ON ur."RoleId" = r."Id"
ORDER BY r."Id", u."UserName";

\echo ''
\echo '📈 Resumen por roles:'
SELECT 
    r."Name" as "Role",
    COUNT(*) as "Total_Users"
FROM "AspNetRoles" r
JOIN "AspNetUserRoles" ur ON r."Id" = ur."RoleId"
GROUP BY r."Name", r."Id"
ORDER BY r."Id";
EOSQL

echo ""
echo "✅ Proceso completado exitosamente!"
echo ""
echo "📝 RESUMEN:"
echo "   ✅ Roles antiguos eliminados"
echo "   ✅ Nuevos roles creados (superadmin=1, admin=2, user=3)"
echo "   ✅ TODOS los usuarios tienen rol 'user' (base)"
echo "   ✅ Usuarios específicos tienen roles adicionales según configuración"
echo "   ✅ Sistema listo para usar"
echo ""
echo "🎭 ESTRUCTURA FINAL DE ROLES:"
echo "   👑 SuperAdmins: rol 'user' + rol 'superadmin'"
echo "   🛡️  Admins: rol 'user' + rol 'admin'"
echo "   👤 Otros usuarios: solo rol 'user'"
echo ""
echo "🚀 PRÓXIMOS PASOS:"
echo "   1. Verificar que los usuarios pueden hacer login"
echo "   2. Probar funcionalidades de admin"
echo "   3. Verificar que los SuperAdmins pueden gestionar roles"
echo "   4. Ajustar roles adicionales si es necesario"
