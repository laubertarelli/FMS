<script setup>
import http from "@/shared/http";
import { formatDate } from "@/shared/formatters";
import { onMounted, reactive, ref } from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute();
const router = useRouter();
const procedure = ref({});
const labels = reactive([]);
const newLabel = ref("");
const isUpdating = ref(false);

onMounted(async () => {
    try {
        const id = parseInt(route.params.id);
        procedure.value = (await http.get(`procedures/details/${id}`)).data;

        const result = await http.get("procedures/labels");
        labels.splice(0, labels.length, ...result.data);
    } catch {
        router.replace({ name: "Procedures", params: { id: 1 } });
    }
});

async function update() {
    try {
        procedure.value.label = newLabel.value;
        await http.put(`procedures/${procedure.value.id}`, procedure.value);
    } catch(e) {
        console.log(e);
    }
}

const isCurrentLabel = (label) => label === procedure.value.label;
</script>

<template>
    <div class="div-container form-details">
        <h2 id="heading">Procedure #{{ procedure.id }}</h2>
        <div class="data-container">
            <div class="content-container">
                <label for="content">Content</label>
                <input :disabled="!isUpdating" class="input" type="text" id="content" v-model="procedure.content" />
            </div>
            <div>
                <label for="label">Label</label>
                <input v-if="!isUpdating" disabled class="input" type="text" id="label" :value="procedure.label" />
                <select v-else class="input" id="label" v-model="newLabel">
                    <option value="" disabled selected>Select a label</option>
                    <option v-for="label in labels" :key="label.value" :value="label.value">{{ label.name }} {{ isCurrentLabel(label.name) ? "(current)" : "" }}</option>
                </select>
            </div>
            <div>
                <label for="user">Last Update User</label>
                <input disabled class="input" type="text" id="user" :value="procedure.userFullName" />
            </div>
            <div>
                <label for="edition">Last Update Date</label>
                <input disabled class="input" type="text" id="edition" :value="formatDate(procedure.updatedOn)" />
            </div>
            <div>
                <label for="creation">Creation Date</label>
                <input disabled class="input" type="text" id="creation" :value="formatDate(procedure.createdOn)" />
            </div>
            <div v-if="!isUpdating" class="div-btn">
                <RouterLink :to="`/files/details/${procedure.fileId}`" class="form-btn grey">File container</RouterLink>
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

.content-container {
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