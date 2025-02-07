<script setup>
import http from '@/shared/http';
import { onMounted, reactive } from 'vue';

const procedure = reactive({
    content: "",
    label: "",
    fileId : "",
});
const labels = reactive([]);
const fileIds = reactive([]);

onMounted(async () => {
    try {
        const labelsResult = await http.get("procedures/labels");
        labels.splice(0, labels.length, ...labelsResult.data);
        const idsResult = await http.get("files/ids");
        fileIds.splice(0, fileIds.length, ...idsResult.data);
    } catch(e) {
        console.log(e);
    }
});

async function add() {
    await http.post("procedures", procedure);
    //router.push(`procedures/${Math.ceil(procedure.id / 5.0)}`);
}
</script>

<template>
    <form class="form form-details" @submit.prevent="add">
        <p id="heading">Add Procedure</p>
        <div class="field">
            <input required class="input-field" placeholder="Content" id="content" v-model="procedure.content">
        </div>
        <div class="field">
            <select required class="input-field" id="label" v-model="procedure.label">
                <option value="" disabled selected>Select a label</option>
                <option v-for="label in labels" :key="label.value" :value="label.value">{{ label.name }}</option>
            </select>
        </div>
        <div class="field">
            <select required class="input-field" id="file-id" v-model="procedure.fileId">
                <option value="" disabled selected>Select a file id</option>
                <option v-for="id in fileIds" :key="id">{{ id }}</option>
            </select>
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