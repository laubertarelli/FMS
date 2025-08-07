<template>
  <nav class="breadcrumbs" aria-label="Breadcrumb">
    <ol class="breadcrumb-list">
      <li class="breadcrumb-item">
        <router-link to="/" class="breadcrumb-link">
          🏠 Home
        </router-link>
      </li>
      <li v-for="(crumb, index) in breadcrumbs" :key="index" class="breadcrumb-item">
        <span class="breadcrumb-separator">></span>
        <router-link 
          v-if="crumb.to && index < breadcrumbs.length - 1" 
          :to="crumb.to" 
          class="breadcrumb-link">
          {{ crumb.text }}
        </router-link>
        <span v-else class="breadcrumb-current">
          {{ crumb.text }}
        </span>
      </li>
    </ol>
  </nav>
</template>

<script setup>
import { computed, defineOptions } from 'vue';
import { useRoute } from 'vue-router';

// Definir el nombre del componente para evitar el warning de ESLint
defineOptions({
  name: 'AppBreadcrumbs'
});

const route = useRoute();

// Computed para generar breadcrumbs dinámicamente
const breadcrumbs = computed(() => {
  const crumbs = [];
  const routeName = route.name;
  const params = route.params;

  // Mapeo de rutas a breadcrumbs
  switch (routeName) {
    case 'Files':
      crumbs.push({ text: 'Files', to: `/files/${params.page || 1}` });
      break;
      
    case 'File Details':
      crumbs.push({ text: 'Files', to: '/files/1' });
      crumbs.push({ text: `File ${params.id}`, to: null });
      break;
      
    case 'Add File':
      crumbs.push({ text: 'Files', to: '/files/1' });
      crumbs.push({ text: 'Add New File', to: null });
      break;
      
    case 'Procedures':
      crumbs.push({ text: 'Procedures', to: `/procedures/${params.page || 1}` });
      break;
      
    case 'Procedure Details':
      crumbs.push({ text: 'Procedures', to: '/procedures/1' });
      crumbs.push({ text: `Procedure ${params.id}`, to: null });
      break;
      
    case 'Add Procedure':
      crumbs.push({ text: 'Procedures', to: '/procedures/1' });
      crumbs.push({ text: 'Add New Procedure', to: null });
      break;
      
    case 'Users':
      crumbs.push({ text: 'Users', to: `/users/${params.page || 1}` });
      break;
      
    case 'User Details':
      crumbs.push({ text: 'Users', to: '/users/1' });
      crumbs.push({ text: `User ${params.id}`, to: null });
      break;
      
    case 'Manage Claims':
      crumbs.push({ text: 'Users', to: '/users/1' });
      crumbs.push({ text: `User ${params.id}`, to: `/users/details/${params.id}` });
      crumbs.push({ text: 'Manage Claims', to: null });
      break;
      
    case 'Account':
      crumbs.push({ text: 'Account', to: null });
      break;
      
    default:
      // Para rutas no mapeadas, usar el nombre de la ruta
      if (routeName && routeName !== 'Home') {
        crumbs.push({ text: routeName, to: null });
      }
  }

  return crumbs;
});
</script>

<style scoped>
.breadcrumbs {
  background-color: #f8f9fa;
  padding: 0.75rem 1rem;
  border-radius: 0.375rem;
  margin-bottom: 1rem;
  border: 1px solid #e5e7eb;
}

.breadcrumb-list {
  display: flex;
  align-items: center;
  margin: 0;
  padding: 0;
  list-style: none;
  flex-wrap: wrap;
}

.breadcrumb-item {
  display: flex;
  align-items: center;
}

.breadcrumb-separator {
  margin: 0 0.5rem;
  color: #6b7280;
  font-weight: normal;
}

.breadcrumb-link {
  color: #3b82f6;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.2s;
}

.breadcrumb-link:hover {
  color: #1d4ed8;
  text-decoration: underline;
}

.breadcrumb-current {
  color: #374151;
  font-weight: 600;
}

/* Tema oscuro para que combine con tu aplicación */
@media (prefers-color-scheme: dark) {
  .breadcrumbs {
    background-color: #374151;
    border-color: #4b5563;
  }
  
  .breadcrumb-link {
    color: #60a5fa;
  }
  
  .breadcrumb-link:hover {
    color: #93c5fd;
  }
  
  .breadcrumb-current {
    color: #f3f4f6;
  }
  
  .breadcrumb-separator {
    color: #9ca3af;
  }
}
</style>
