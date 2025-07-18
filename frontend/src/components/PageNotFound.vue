<template>
    <div class="d-flex align-items-center justify-content-center">
        <div class="text-center">
            <h1 class="display-1 fw-bold">404</h1>
            <p class="fs-3"> <span class="text-danger">Opps!</span> Page not found.</p>
            <p class="lead">The page you’re looking for doesn’t exist.</p>
            <RouterLink :to="{ name: 'Home' }" class="nav-link">Go Home</RouterLink>
        </div>
    </div>
</template>

<style scoped>
.nav-link {
  background-color: #0066ff;
  border-radius: 10px;
  display: inline-block;
  color: white;
  padding: 7px 13px;
  font-size: 18px;
  font-weight: 500;
}
</style>