<script setup>
import router from "@/router";
import http from "@/shared/http";
import { reactive } from "vue";

const model = reactive({
    email: "",
    password: "",
    confirmPassword: ""
});

async function resetPassword() {
    await http.put("reset-password", model);
};
</script>

<template>
    <form class="form form-details" @submit.prevent="resetPassword">
        <p id="heading">Reset Password</p>
        <div class="field">
            <input required class="input-field" type="email" placeholder="Email" id="email" v-model="model.email">
        </div>
        <div class="field">
            <input required minlength="8" class="input-field" type="password" placeholder="Password" id="password" v-model="model.password">
        </div>
        <div class="field">
            <input required minlength="8" class="input-field" type="password" placeholder="Confirm Password" id="confirmPassword" v-model="model.confirmPassword">
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