<script setup>
import http from "@/shared/http";
import { formatDate } from "@/shared/formatters";
import { onMounted, ref, reactive } from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute();
const router = useRouter();
const file = ref({});
const states = reactive([]);
const newState = ref("");
const isUpdating = ref(false);

onMounted(async () => {
    try {
        const id = parseInt(route.params.id);
        file.value = (await http.get(`files/details/${id}`)).data;

        const result = await http.get("files/states");
        states.splice(0, states.length, ...result.data);
    } catch {
        router.replace({ name: "Files", params: { page: 1 } });
    }
});

async function update() {
    try {
        file.value.state = newState.value;
        await http.put(`files/${file.value.id}`, file.value);
    } catch(e) {
        console.log(e);
    }
}

const isCurrentState = (state) => state === file.value.state;
</script>

<template>
    <div class="div-container form-details">
        <h2 id="heading">File #{{ file.id }}</h2>
        <div class="data-container">
            <div class="cover-container">
                <label for="cover">Cover</label>
                <input :disabled="!isUpdating" class="input" type="text" id="cover" v-model="file.cover" />
            </div>
            <div>
                <label for="state">State</label>
                <input v-if="!isUpdating" disabled class="input" type="text" id="state" :value="file.state" />
                <select v-else class="input" id="state" v-model="newState">
                    <option value="" disabled selected>Select a state</option>
                    <option v-for="state in states" :key="state.value" :value="state.value">{{ state.name }} {{ isCurrentState(state.name) ? "(current)" : "" }}</option>
                </select>
            </div>
            <div>
                <label for="user">Last Update User</label>
                <input disabled class="input" type="text" id="user" :value="file.userFullName" />
            </div>
            <div>
                <label for="edition">Last Update Date</label>
                <input disabled class="input" type="text" id="edition" :value="formatDate(file.updatedOn)" />
            </div>
            <div>
                <label for="creation">Creation Date</label>
                <input disabled class="input" type="text" id="creation" :value="formatDate(file.createdOn)" />
            </div>
            <div v-if="!isUpdating" class="div-btn">
                <RouterLink :to="`/procedures/1/file/${file.id}`" class="form-btn grey">Procedures</RouterLink>
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

.div-btn {
    display: flex;
    justify-content: space-between;
    grid-column: span 2 / span 2;
}
</style>