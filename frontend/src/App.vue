<script setup>
import { onMounted, ref } from 'vue';
import NavMenu from './components/NavMenu.vue';

const hasToken = ref(false);
const checkToken = () => {
  hasToken.value = localStorage.getItem('token') !== null;
};

onMounted(() => checkToken());
</script>

<template>
  <div class="page">
    <header>
      <RouterLink class="home-link" :to="{ name: 'Home' }">
        <img alt="logo" src="./assets/logo.png" />
        <span class="logo">FMS</span>
      </RouterLink>
      <div v-if="hasToken">
        <NavMenu />
      </div>
    </header>
    <main>
      <RouterView />
    </main>
    <footer>
      <div class="footer-container">
        <div class="footer-info">
          <a class="footer-title">
            <span>FMS</span>
          </a>
        </div>
      </div>
    </footer>
  </div>
</template>

<style>
.page {
  display: flex;
  flex-direction: column;
  height: 100vh;
  background-color: #111827;
  color: #f5f5f4;
}

td {
  vertical-align: middle;
}

header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 10%;
  background-color: #1f2937;
}

header img {
  cursor: pointer;
  height: 45px;
  width: 45px;
}

.home-link {
  display: flex;
  gap: 0.5rem;
  align-items: center;
  text-decoration: none;
}

.logo {
  font-size: 2rem;
  font-weight: bold;
  color: white;
}

main {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  flex-basis: 300px;
  flex-grow: 2;
  flex-shrink: 0;
}

footer {
  display: flex;
}

.footer-container {
  background-color: #1f2937;
  display: flex;
  flex-wrap: wrap;
  flex-direction: column;
  flex-grow: 0.001;
  gap: 2rem;
  padding: 1rem;
  width: 100%;
  margin-left: auto;
  margin-right: auto;
}

.footer-info {
  text-align: center;
}

.footer-title {
  display: flex;
  align-items: center;
  justify-content: center;
  text-decoration: none;
  font-size: 1.125rem;
  line-height: 1.75rem;
  font-weight: 500;
  margin-bottom: 0.5rem;
  color: white;
}
</style>