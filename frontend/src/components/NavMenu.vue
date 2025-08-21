<script setup>
import { isAdmin, isGuest, removeToken } from '@/shared/auth';
import { useRouter } from 'vue-router';
import http from '@/shared/http';

const router = useRouter();

async function logout() {
    try {
        await http.logout();
        removeToken();
        // Notificar al App.vue que el estado de autenticación cambió
        if (window.updateAuthState) {
            window.updateAuthState();
        }
        router.replace({ name: "Login" });
    } catch (e) {
        console.log(e);
    }
}
</script>

<template>
    <div>
        <nav class="div-nav">
            <ul class="nav-links">
                <li>
                    <RouterLink to="/files/1">Files</RouterLink>
                </li>
                <li>
                    <RouterLink to="/procedures/1">Procedures</RouterLink>
                </li>
                <li v-if="isAdmin() || isGuest()">
                    <RouterLink to="/users/1">Users</RouterLink>
                </li>
            </ul>
            <RouterLink v-if="!isGuest()" :to="{ name: 'Account' }"><button class="btn-header">Account</button></RouterLink>
            <button v-else @click="logout" class="btn btn-danger p-2 logout">Log out</button>
        </nav>
    </div>
</template>

<style>
.div-nav {
    display: flex;
    align-items: baseline;
}

.nav-links {
    list-style: none;
}

.nav-links li {
    display: inline-block;
    padding: 0px 18px;
    font-weight: 500;
    font-size: 16px;
    color: #edf0f1;
    text-decoration: none;
}

.nav-links li a {
    transition: all 0.3s ease 0;
    font-weight: 500;
    font-size: 16px;
    color: #edf0f1;
    text-decoration: none;
}

.btn-header {
    margin-left: 20px;
    padding: 9px 25px;
    background-color: #1b6ec2;
    border: none;
    border-radius: 50px;
    cursor: pointer;
    font-weight: 500;
    font-size: 16px;
    color: #edf0f1;
    text-decoration: none;
}

.btn-header:hover {
    background-color: #0b5ed7;
}

.logout {
    width: 94px;
    margin-left: 1rem;
}
</style>