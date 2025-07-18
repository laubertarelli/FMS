<script setup>
import http from "@/shared/http";
import { onMounted, reactive, ref } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const claims = reactive([]);
const userClaims = reactive([]);
const isUpdating = ref(false);

onMounted(async () => {
    try {
        const id = route.params.id;
        let result = await http.get("users/claims");
        claims.splice(0, claims.length, ...result.data);

        result = await http.get(`users/claims/${id}`);
        userClaims.splice(0, userClaims.length, ...result.data);
    } catch (e) {
        console.error(e);
    }
});

async function update() {
    try {
        await http.put("claims", {
            id: route.params.id,
            claims: userClaims
        });
        isUpdating.value = !isUpdating.value;
    } catch (e) {
        console.error(e);
    }
}

const grant = (name) => userClaims.push({ value: name.toLowerCase() });
function remove(name) {
    const index = userClaims.findIndex(c => c.value === name.toLowerCase());
    if (index !== -1) {
        userClaims.splice(index, 1);
    }
};
</script>

<template>
    <div class="div-container">
        <h2 id="heading">User Permissions</h2>
        <table class="table text-white p-2">
            <thead>
                <tr>
                    <th>Permission</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="claim in claims" :key="claim.value">
                    <td>{{ claim.name }}</td>
                    <td>
                        <button v-if="userClaims.some(c => c.value === claim.name.toLowerCase())"
                            class="btn btn-danger width" @click="remove(claim.name)">Remove</button>
                        <button v-else class="btn btn-success width" @click="grant(claim.name)">Grant</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<style>
.div-container {
    background-color: #1f2937;
    width: 35%;
    padding: 1.3rem 2rem;
    border-radius: 10px;
}

#heading {
    text-align: center;
    margin: 1rem;
    color: rgb(255, 255, 255);
    font-size: 1.2rem;
}

.width {
    width: 88px;
}
</style>