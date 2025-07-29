<script setup>
import http from "@/shared/http";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const user = ref({});
const isUpdating = ref(false);

onMounted(async () => {
    try {
        user.value = (await http.get("account")).data;
    } catch {
        router.replace({ name: "Users", params: { id: 1 } });
    }
});

async function update() {
    try {
        await http.put("account/update", user.value);
        isUpdating.value = !isUpdating.value;
    } catch (e) {
        console.log(e);
    }
}

async function logout() {
    try {
        await http.logout();
        localStorage.removeItem("token");
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
    <div class="div-container form-details">
        <div class="data-container">
            <div class="div-btn mt-3 align-items-center">
                <h2 id="heading" class="m-0">My Account</h2>
                <button @click="logout" class="btn btn-danger p-2 logout">Log out</button>
            </div>
            <div class="username-container">
                <label for="username">Username</label>
                <input :disabled="!isUpdating" class="input" type="text" id="username" v-model="user.username" />
            </div>
            <div>
                <label for="email">Email</label>
                <input :disabled="!isUpdating" class="input" type="text" id="email" v-model="user.email" />
            </div>
            <div>
                <label for="first-name">First Name</label>
                <input :disabled="!isUpdating" class="input" type="text" id="first-name" v-model="user.firstName" />
            </div>
            <div>
                <label for="last-name">Last Name</label>
                <input :disabled="!isUpdating" class="input" type="text" id="last-name" v-model="user.lastName" />
            </div>
            <div>
                <label for="files">Files Created</label>
                <input disabled class="input" type="text" id="files" :value="user.files" />
            </div>
            <div>
                <label for="procedures">Procedures Created</label>
                <input disabled class="input" type="text" id="procedures" :value="user.procedures" />
            </div>
            <div v-if="!isUpdating" class="div-btn">
                <RouterLink :to="{ name: 'Forgot Password' }" class="form-btn grey">Change password</RouterLink>
                <button @click="() => isUpdating = !isUpdating" class="form-btn">Update</button>
            </div>
            <div v-else class="div-btn">
                <button @click="() => isUpdating = !isUpdating" class="form-btn grey">Cancel</button>
                <button @click="update" class="form-btn">Save changes</button>
            </div>
        </div>
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

.m-0 {
    margin: 0 !important;
  }

label {
    font-weight: 500;
    font-size: 0.875rem;
    line-height: 1.25rem;
    margin-bottom: 0.5rem;
}

.cover-container {
    grid-column: span 2 / span 2;
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

.logout {
    width: 94px;
}
</style>