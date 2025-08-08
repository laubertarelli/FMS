<script setup>
import http from '@/shared/http';
import { reactive } from 'vue';

const user = reactive({
    username: "",
    email: "",
    firstName: "",
    lastName: "",
    password: "",
    confirmPassword: ""
});

async function signup() {
    await http.post("signup", user);
}
</script>

<template>
    <div class="div-container form-details">
        <h2 id="heading">Sign Up</h2>
        <form class="data-container" @submit.prevent="signup">
            <div>
                <label for="username">Username</label>
                <input required class="input" type="text" placeholder="Username" id="username" v-model="user.username">
            </div>
            <div>
                <label for="email">Email</label>
                <input required class="input" type="email" placeholder="Email" id="email" v-model="user.email">
            </div>
            <div>
                <label for="first-name">First Name</label>
                <input required class="input" type="text" placeholder="First name" id="first-name" v-model="user.firstName">
            </div>
            <div>
                <label for="last-name">Last Name</label>
                <input required class="input" type="text" placeholder="Last name" id="last-name" v-model="user.lastName">
            </div>
            <div>
                <label for="passwd">Password</label>
                <input required minlength="8" class="input" type="password" placeholder="Password" id="passwd" v-model="user.password">
            </div>
            <div>
                <label for="conf-passwd">Confirm Password</label>
                <input required minlength="8" class="input" type="password" placeholder="Confirm password" id="conf-passwd" v-model="user.confirmPassword">
            </div>
            <div class="div-btn">
                <button type="submit" class="form-btn">Accept</button>
            </div>
        </form>
    </div>
</template>

<style>
.div-container {
    background-color: #1f2937;
    width: 42rem;
    margin-left: auto;
    margin-right: auto;
}

.data-container {
    display: grid;
    gap: 1.5rem;
    grid-template-columns: repeat(2, minmax(0, 1fr));
}

label {
    font-weight: 500;
    font-size: 0.875rem;
    line-height: 1.25rem;
    margin-bottom: 0.5rem;
}

.input {
    display: block;
    background-color: rgb(55 65 81);
    border: none;
    border-radius: 0.5rem;
    font-size: 0.875rem;
    line-height: 1.25rem;
    padding: 0.625rem;
    outline: none;
    width: 100%;
    color: #d3d3d3;
}

.div-btn {
    display: flex;
    justify-content: space-between;
    grid-column: span 2 / span 2;
}
</style>