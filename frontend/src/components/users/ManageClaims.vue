<script setup>
import http from "@/shared/http";
import { onMounted, reactive } from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute();
const router = useRouter();
const allClaims = reactive([]);
const userClaims = reactive([]);

onMounted(async () => {
    try {
        const id = route.params.id;
        let result = await http.get("users/claims");
        allClaims.splice(0, allClaims.length, ...result.data);

        result = await http.get(`users/claims/${id}`);
        userClaims.splice(0, userClaims.length, ...result.data);
    } catch {
        router.replace({ name: "Users", params: { id: 1 } });
    }
});

const add = (name) => userClaims.push({ value: name.toLowerCase() });
function remove(name) {
    const index = userClaims.findIndex(c => c.value === name.toLowerCase());
    if (index !== -1) {
        userClaims.splice(index, 1);
    }
};

async function update() {
    try {
        // Preparar los claims en el formato que espera el backend
        const claimsToSend = allClaims.map(claim => ({
            claimValue: claim.name.toLowerCase(),
            isSelected: userClaims.some(userClaim => userClaim.value === claim.name.toLowerCase())
        }));

        await http.put("users/claims", { 
            userId: route.params.id, 
            claims: claimsToSend 
        });
        location.reload();
    } catch(e) {
        console.log(e);
    }
}
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
                <tr v-for="claim in allClaims" :key="claim.value">
                    <td>{{ claim.name }}</td>
                    <td>
                        <button v-if="userClaims.some(c => c.value === claim.name.toLowerCase())"
                            class="btn btn-danger width" @click="remove(claim.name)">Remove</button>
                        <button v-else class="btn btn-success width" @click="add(claim.name.toLowerCase())">Add</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="div-btn">
            <button @click="update" class="form-btn">Update</button>
        </div>
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