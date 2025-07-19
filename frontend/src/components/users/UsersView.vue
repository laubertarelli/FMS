<script setup>
import { onMounted, reactive, ref, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import http from "@/shared/http";
import DeleteCard from "../cards/DeleteCard.vue";

const route = useRoute();
const router = useRouter();

const page = ref(parseInt(route.params.page) ?? 1);
const totalUsers = ref(0);
const totalPages = ref(0);
const users = reactive([]);

const hasPrevious = computed(() => page.value > 1);
const hasNext = computed(() => page.value < totalPages.value);

onMounted(async () => {
    try {
        totalUsers.value = (await http.get("users/count")).data;
        totalPages.value = Math.ceil(totalUsers.value / 5.0);
        redirect();
        const result = await http.get(`users/${page.value}`);
        users.splice(0, users.length, ...result.data);
    } catch (e) {
        console.error(e);
        // Si hay error de autenticación, redirigir al login
        if (e.response?.status === 401) {
            localStorage.removeItem('token');
            router.push('/');
        }
    }
});

function redirect() { // VER SI SE PUEDE IMPLEMENTAR EN ROUTER/INDEX.JS
    if (page.value < 1)
        router.push(`/users/1`);
    if (totalPages.value > 0 && page.value > totalPages.value)
        router.push(`/users/${totalPages.value}`);
};

const handleDeleteClick = (id) => {
    modals.deleteModal = true;
    modals.id = id
};

const modals = reactive({
    deleteModal: false,
    id: 0
});
</script>

<template>
    <div class="table-container">
        <table class="table text-white p-2 text-center">
            <thead>
                <tr>
                    <th class="text-start">Username</th>
                    <th>Full Name</th>
                    <th>Email</th>
                    <th>Files</th>
                    <th>Procedures</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="u in users" :key="u.id">
                    <td class="text-start">{{ u.username }}</td>
                    <td>{{ u.fullName }}</td>
                    <td>{{ u.email }}</td>
                    <td>{{ u.files }}</td>
                    <td>{{ u.procedures }}</td>
                    <td>
                        <RouterLink class="btn btn-success" :to="`/users/details/${u.id}`">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                class="bi bi-eye-fill" viewBox="0 0 16 16">
                                <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                <path
                                    d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                            </svg>
                        </RouterLink>
                        <!-- <RouterLink class="btn btn-pencil" :to="`/users/1`">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="white"
                                class="bi bi-pencil-fill" viewBox="0 0 16 16">
                                <path
                                    d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.5.5 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11z" />
                            </svg>
                        </RouterLink> -->
                        <button class="btn btn-danger" @click="handleDeleteClick(u.id)">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                class="bi bi-trash3-fill" viewBox="0 0 16 16">
                                <path
                                    d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5" />
                            </svg>
                        </button>
                        
                    </td>
                </tr>
            </tbody>
        </table>
        <nav class="nav">
            <span>Mostrando {{ totalUsers > 0 ? ((page - 1) * 5) + 1 : "0" }} - {{ (page * 5) < totalUsers ? page * 5 :
                totalUsers }} de {{ totalUsers }}</span>
                    <ul v-if="totalPages > 1">
                        <li>
                            <a v-if="hasPrevious" class="element prev" :href="`/users/${page - 1}`">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor"
                                    class="bi bi-arrow-left-short" viewBox="0 0 16 16">
                                    <path fill-rule="evenodd"
                                        d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5" />
                                </svg>
                            </a>
                        </li>
                        <li v-for="i in Math.min(totalPages, 5)" :key="i">
                            <a class="element" :class="{ 'num-selected': i === page }" :href="`/users/${i}`">{{ i }}</a>
                        </li>
                        <li>
                            <a v-if="hasNext" class="element next" :href="`/users/${page + 1}`">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor"
                                    class="bi bi-arrow-right-short" viewBox="0 0 16 16">
                                    <path fill-rule="evenodd"
                                        d="M4 8a.5.5 0 0 1 .5-.5h5.793L8.146 5.354a.5.5 0 1 1 .708-.708l3 3a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708-.708L10.293 8.5H4.5A.5.5 0 0 1 4 8" />
                                </svg>
                            </a>
                        </li>
                    </ul>
        </nav>
        <DeleteCard v-if="modals.deleteModal" v-model="modals.deleteModal" :id="modals.id" :type="'users'" />
    </div>
</template>