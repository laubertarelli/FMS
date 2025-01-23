<script setup>
import { onMounted, reactive, ref, computed } from "vue";
import { useRouter } from "vue-router";
import http from "@/http";

const router = useRouter();
const urlPage = ref(1) // URL PARAMS
const page = ref(0);
const totalFiles = ref(0);
const totalPages = ref(0);
const files = reactive([]);

const hasPrevious = computed(() => page.value > 1);
const hasNext = computed(() => page.value < totalPages.value);

onMounted(async () => {
    try {
        totalFiles.value = (await http.get("files/count")).data;
        totalPages.value = Math.ceil(totalFiles.value / 5.0);
        page.value = urlPage.value ?? 1;
        const result = await http.get(`files/${page.value}`);
        files.splice(0, files.length, ...result.data);
    } catch (e) {
        console.error(e);
    }
});

function add() {

}

function update(id) {
    return id;
}

function view(id) {
    return id;
}

function previousPage() {
    if (hasPrevious.value) {
        urlPage.value = page.value - 1;
        router.go(`files/${urlPage.value}`);
    }
}

function nextPage() {
    if (hasNext.value) {
        urlPage.value = page.value + 1;
        router.go(`files/${urlPage.value}`);
    }
}
</script>

<template>
    <div class="div-container">
        <button class="btn btn-primary p-2 mb-3" @click="add">+ Add File</button>
        <table class="table text-white p-2 text-center">
            <thead>
                <tr>
                    <th class="text-start"># - Cover (State)</th>
                    <th>Procedures</th>
                    <th>Last Update</th>
                    <th>Created On</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="f in files" :key="f.id">
                    <td class="text-start">#{{ f.id }} - {{ f.cover }}<br>({{ f.state }})</td>
                    <!-- <td>{{ f.procedures.length }}</td> -->
                    <td>{{ f.userFullName }}<br>{{ f.updatedOn }}</td>
                    <td>{{ f.createdOn }}</td>
                    <td>
                        <button class="btn btn-success" @click="view(f.id)">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                class="bi bi-eye-fill" viewBox="0 0 16 16">
                                <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                                <path
                                    d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
                            </svg>
                        </button>
                        <button class="btn btn-pencil" @click="update(f.id)">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="white"
                                class="bi bi-pencil-fill" viewBox="0 0 16 16">
                                <path
                                    d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.5.5 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11z" />
                            </svg>
                        </button>
                        <!-- <button class="btn btn-danger" @click="_dialogueDelete.Show(f.id)">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                class="bi bi-trash3-fill" viewBox="0 0 16 16">
                                <path
                                    d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5" />
                            </svg>
                        </button> -->
                    </td>
                </tr>
            </tbody>
        </table>
        <nav class="nav">
            <span>Mostrando {{ totalFiles > 0 ? ((page - 1) * 5) + 1 : "0" }} - {{ (page * 5) < totalFiles ? page * 5 : totalFiles }} de {{ totalFiles }}</span>
                    <ul v-if="totalPages > 1">
                        <li>
                            <div class="element prev" href="#" @click="previousPage">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor"
                                    class="bi bi-arrow-left-short" viewBox="0 0 16 16">
                                    <path fill-rule="evenodd"
                                        d="M12 8a.5.5 0 0 1-.5.5H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5a.5.5 0 0 1 .5.5" />
                                </svg>
                            </div>
                        </li>
                        <div v-for="i in Math.min(totalPages, 5)" :key="i">
                            <li v-if="i === page.value">
                                <a class="element num-selected" href="`${files}/${i})`">{{ i }}</a>
                            </li>
                            <li>
                                <a class="element" href="`${files}/${i})`">{{ i }}</a>
                            </li>
                        </div>
                        <li>
                            <div class="element next" href="#" @click="nextPage">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor"
                                    class="bi bi-arrow-right-short" viewBox="0 0 16 16">
                                    <path fill-rule="evenodd"
                                        d="M4 8a.5.5 0 0 1 .5-.5h5.793L8.146 5.354a.5.5 0 1 1 .708-.708l3 3a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708-.708L10.293 8.5H4.5A.5.5 0 0 1 4 8" />
                                </svg>
                            </div>
                        </li>
                    </ul>
        </nav>
    </div>
</template>

<style>
.div-container {
    background-color: #1f2937;
    width: 65%;
    padding: 1.3rem 2rem;
    border-radius: 10px;
}

.btn {
    padding: .275rem .65rem;
}

.btn-pencil {
    background-color: #ff7e22;
}

.btn-pencil:hover {
    background-color: #c55d12;
}

.nav {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin: 0px auto;
    padding: 0.5rem;
}

.nav ul {
    display: inline-flex;
    align-items: stretch;
    margin-left: 1rem;
    margin-bottom: 0;
    list-style: none;
}

.element {
    display: flex;
    cursor: pointer;
    text-decoration: none;
    text-align: center;
    align-items: center;
    justify-content: center;
    height: 100%;
    padding: 0.50rem 0.75rem;
    margin-left: 0;
    color: rgb(156, 163, 175);
    background-color: #1f2937;
    border: 1px solid rgb(55, 65, 81);
}

.element:hover {
    color: white;
    background-color: rgb(55, 65, 81);
}

.prev {
    border-top-left-radius: 0.5rem;
    border-bottom-left-radius: 0.5rem;
}

.num-selected {
    color: white;
    background-color: rgb(55, 65, 81);
}

.next {
    border-top-right-radius: 0.5rem;
    border-bottom-right-radius: 0.5rem;
}
</style>