# Configuración de Roles para FMS
# Edita este archivo antes de ejecutar cleanup-roles.sh

# ========================================
# INSTRUCCIONES:
# ========================================
# 1. TODOS los usuarios obtendrán automáticamente el rol 'user' (base)
# 2. Los usuarios listados aquí obtendrán roles ADICIONALES al rol 'user'
# 3. Reemplaza los emails de ejemplo con los emails reales de tus usuarios
# 4. Puedes dejar las listas vacías si no tienes usuarios para esos roles especiales

# ========================================
# SUPERADMINS (user + superadmin)
# ========================================
# Estos usuarios tendrán AMBOS roles: 'user' + 'superadmin'
# Pueden:
# - Gestionar todos los usuarios
# - Asignar/quitar roles
# - Acceso completo a todas las funcionalidades

SUPERADMIN_EMAILS=(
    "lautaber9@gmail.com"
    # "otro-superadmin@ejemplo.com"
)

# ========================================
# ADMINS (user + admin)
# ========================================
# Estos usuarios tendrán AMBOS roles: 'user' + 'admin'
# Pueden:
# - Gestionar usuarios normales
# - Ver reportes y estadísticas
# - Funcionalidades administrativas (no gestión de roles)

ADMIN_EMAILS=(
    "lautaber9@gmail.com"
    # "admin1@ejemplo.com"
    # "admin2@ejemplo.com"
)

# ========================================
# USUARIOS NORMALES (solo user)
# ========================================
# Todos los demás usuarios obtendrán ÚNICAMENTE el rol 'user'
# No necesitas listarlos aquí, es automático

# ========================================
# RESULTADO FINAL:
# ========================================
# - SuperAdmins: roles 'user' + 'superadmin'
# - Admins: roles 'user' + 'admin'  
# - Otros: solo rol 'user'
#
# NOTAS:
# - Los emails deben coincidir EXACTAMENTE con los registrados en la base de datos
# - Si un email no existe en la DB, será ignorado (no causará error)
# - Puedes ejecutar el script múltiples veces si necesitas hacer ajustes
