<script setup>
import http from '@/shared/http';
import { onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
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
    router.replace("/");
}
</script>

<template>
    <form class="form form-details" @submit.prevent="signup">
        <p id="heading">Sign Up</p>
        <div class="field">
            <input required class="input-field" placeholder="Username" id="username" v-model="user.username">
        </div>
        <div class="field">
            <input required class="input-field" type="email" placeholder="Email" id="email" v-model="user.email">
        </div>
        <div class="field">
            <input required class="input-field" placeholder="First name" id="first-name" v-model="user.firstName">
        </div>
        <div class="field">
            <input required class="input-field" placeholder="Last name" id="last-name" v-model="user.lastName">
        </div>
        <div class="field">
            <input required class="input-field" type="password" placeholder="password" id="" v-model="user.password">
        </div>
        <div class="field">
            <input required class="input-field" type="password" placeholder="Confirm password" id="conf-passwd" v-model="user.confirmPassword">
        </div>
        <div class="div-btn">
            <button type="submit" class="form-btn">Accept</button>
        </div>
    </form>
</template>

<style>
.div-btn {
    padding: .375rem .75rem;
}
</style>