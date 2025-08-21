<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import http from "@/shared/http";
import { setToken } from "@/shared/auth";
import WarningCard from '@/components/cards/WarningCard.vue';

const email = ref("");
const password = ref("");
const router = useRouter();
const showGuestWarning = ref(false);

async function login() {
    const response = await http.login({
        email: email.value,
        password: password.value
    });
    if (response?.status === 200) {
        setToken(response.data.token);
        // Notificar al App.vue que el estado de autenticación cambió
        if (window.updateAuthState) {
            window.updateAuthState();
        }
        router.replace({ name: "Home" });
    }
}

function showGuestWarningModal() {
    showGuestWarning.value = true;
}

async function confirmGuestLogin() {
    const response = await http.loginAsGuest();
    if (response?.status === 200) {
        setToken(response.data.token);
        // Notificar al App.vue que el estado de autenticación cambió
        if (window.updateAuthState) {
            window.updateAuthState();
        }
        router.replace({ name: "Home" });
    }
}
</script>

<template>
    <form class="form form-details" @submit.prevent="login">
        <p id="heading">Welcome!</p>
        <div class="field">
            <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                viewBox="0 0 16 16">
                <path
                    d="M13.106 7.222c0-2.967-2.249-5.032-5.482-5.032-3.35 0-5.646 2.318-5.646 5.702 0 3.493 2.235 5.708 5.762 5.708.862 0 1.689-.123 2.304-.335v-.862c-.43.199-1.354.328-2.29.328-2.926 0-4.813-1.88-4.813-4.798 0-2.844 1.921-4.881 4.594-4.881 2.735 0 4.608 1.688 4.608 4.156 0 1.682-.554 2.769-1.416 2.769-.492 0-.772-.28-.772-.76V5.206H8.923v.834h-.11c-.266-.595-.881-.964-1.6-.964-1.4 0-2.378 1.162-2.378 2.823 0 1.737.957 2.906 2.379 2.906.8 0 1.415-.39 1.709-1.087h.11c.081.67.703 1.148 1.503 1.148 1.572 0 2.57-1.415 2.57-3.643zm-7.177.704c0-1.197.54-1.907 1.456-1.907.93 0 1.524.738 1.524 1.907S8.308 9.84 7.371 9.84c-.895 0-1.442-.725-1.442-1.914z">
                </path>
            </svg>
            <input required autocomplete="off" placeholder="Email" class="input-field" type="email" id="email"
                v-model="email">
        </div>
        <div class="field">
            <svg class="input-icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                viewBox="0 0 16 16">
                <path
                    d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2zm3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2z">
                </path>
            </svg>
            <input required placeholder="Password" class="input-field" type="password" id="password" v-model="password">
        </div>
        <div class="div-btn">
            <a :href="`/signup`" class="form-btn grey">Signup</a>
            <button type="submit" class="form-btn">Login</button>
        </div>
        <div class="divider">
            <span class="divider-text">or</span>
        </div>
        <div class="guest-login">
            <button type="button" class="guest-btn" @click="showGuestWarningModal">
                <svg class="guest-icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                    <path d="M8 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm2-3a2 2 0 1 1-4 0 2 2 0 0 1 4 0Zm4 8c0 1-1 1-1 1H3s-1 0-1-1 1-4 6-4 6 3 6 4Zm-1-.004c-.001-.246-.154-.986-.832-1.664C11.516 10.68 10.289 10 8 10c-2.29 0-3.516.68-4.168 1.332-.678.678-.83 1.418-.832 1.664h10Z"/>
                </svg>
                Continue as a guest
            </button>
        </div>
    </form>

    <WarningCard 
      v-model="showGuestWarning"
      title="Guest Access"
      message="As a guest, you can view all files and procedures but cannot create, edit, or delete any content."
      @confirmAction="confirmGuestLogin" />
</template>

<style scoped>
.form {
    transition: .4s ease-in-out;
}

.form:hover {
    transform: scale(1.05);
    border: 1px solid black;
}

.input-icon {
    height: 1.3em;
    width: 1.3em;
    fill: white;
}

.form-btn {
    transition: .4s ease-in-out;
}

.secondary-btn {
    padding: 0.5em;
    padding-left: 2.3em;
    padding-right: 2.3em;
    border-radius: 5px;
    border: none;
    outline: none;
    transition: .4s ease-in-out;
    background-color: #6C757D;
    color: white;
}

.guest-login {
    margin-top: 0.5rem;
    text-align: center;
}

.divider {
    position: relative;
    margin: 0.75rem 0 0.5rem 0;
    text-align: center;
    height: 1px;
    background: #e9ecef;
}

.divider-text {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background: white;
    padding: 0 0.75rem;
    color: #666;
    font-size: 0.85rem;
    font-weight: 500;
    letter-spacing: 0.5px;
}

.guest-btn {
    display: inline-flex;
    align-items: center;
    gap: 0.5rem;
    padding: 0.75rem 1.5rem;
    background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
    border: 2px solid #dee2e6;
    border-radius: 8px;
    color: #495057;
    font-size: 0.95rem;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s ease;
    text-decoration: none;
    min-width: 200px;
    justify-content: center;
}

.guest-btn:hover {
    background: linear-gradient(135deg, #e9ecef 0%, #dee2e6 100%);
    border-color: #adb5bd;
    transform: translateY(-1px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    color: #343a40;
}

.guest-btn:active {
    transform: translateY(0);
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.guest-icon {
    width: 18px;
    height: 18px;
    opacity: 0.7;
    transition: opacity 0.3s ease;
}

.guest-btn:hover .guest-icon {
    opacity: 1;
}
</style>