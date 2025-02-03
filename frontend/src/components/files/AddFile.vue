<script setup>
import http from '@/shared/http';
import { onMounted, reactive } from 'vue';

const file = reactive({
    cover: "",
    state: ""
});
const states = reactive([]);

onMounted(async () => {
    try {
        const result = await http.get("files/states");
        states.splice(0, states.length, ...result.data); 
    } catch(e) {
        console.log(e);
    }
});

async function add() {
    await http.post("files", file);
    //router.push(`files/${Math.ceil(file.id / 5.0)}`);
}
</script>

<template>
    <form class="form" @submit.prevent="add">
        <p id="heading">Add File</p>
        <div class="field">
            <input required class="input-field" placeholder="Cover" id="cover" v-model="file.cover">
        </div>
        <div class="field">
            <select required class="input-field" id="state" v-model="file.state">
                <option value="" disabled selected>Select a state</option>
                <option v-for="state in states" :key="state.value" :value="state.value">{{ state.name }}</option>
            </select>
        </div>
        <div class="div-btn">
            <button type="submit" class="primary-btn">Accept</button>
        </div>
    </form>
</template>

<style>
    .div-btn {
        padding: .375rem .75rem;
    }
</style>