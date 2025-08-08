#!/bin/bash

# Script de despliegue para FMS
# Este script construye la imagen Docker y la sube al registry

set -e

echo "🚀 Iniciando proceso de despliegue..."

# Variables
IMAGE_NAME="ghcr.io/laubertarelli/fms-api"
TAG="latest"

echo "📦 Construyendo imagen Docker..."
docker build -t $IMAGE_NAME:$TAG .

echo "📤 Subiendo imagen al registry..."
docker push $IMAGE_NAME:$TAG

echo "✅ Imagen subida exitosamente!"
echo ""
echo "🔄 OPCIONES DE DESPLIEGUE EN VPS:"
echo ""
echo "📌 OPCIÓN 1 - Limpieza Total (⚠️  BORRA TODOS LOS DATOS):"
echo "   docker-compose -f docker-compose.deploy.yml pull"
echo "   docker-compose -f docker-compose.deploy.yml down"
echo "   docker volume rm fms_pgdata"
echo "   docker-compose -f docker-compose.deploy.yml up -d"
echo ""
echo "📌 OPCIÓN 2 - Limpieza Selectiva (✅ CONSERVA USUARIOS Y DATOS):"
echo "   1. Subir cleanup-roles.sh y role-config.sh al VPS"
echo "   2. Editar role-config.sh con tus emails de admin"
echo "   3. chmod +x cleanup-roles.sh"
echo "   4. ./cleanup-roles.sh"
echo ""
echo "💡 RECOMENDADO: Usar Opción 2 si ya tienes usuarios registrados"
