<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import http from '@/shared/http';
import { onClickOutside } from '@vueuse/core';

const deleteModalRef = ref(null);
const emits = defineEmits(["update:modelValue"]);
const props = defineProps({
    modelValue: {
        type: Boolean,
        required: true
    },
    id: {
        required: true
    },
    type: {
        type: String,
        required: true
    }
});

onMounted(() => {
    document.addEventListener('keyup', handleClose);
});
onUnmounted(() => {
    document.removeEventListener('keyup', handleClose);
});

const closeModal = () => {
    emits('update:modelValue', false);
};
onClickOutside(deleteModalRef, closeModal);

const handleClose = (event) => {
    if (event.key === 'Escape') {
        closeModal();
    }
};

async function deleteItem() {
    try {
        await http.delete(props.type, props.id);
        closeModal();
    } catch (e) {
        console.error(e);
    }
}
</script>

<template>
    <div class="delete-card">
        <div class="card-content" ref="deleteModalRef">
            <p class="card-heading">Are you sure? {{ props.type === 'files' ? 'This process cannot be undone!' : '' }}</p>
            <p v-if="props.type === 'files'" class="card-description">All procedures attached will be deleted.</p>
            <p v-else class="card-description">This process cannot be undone!</p>
            <footer class="card-button-wrapper">
                <button class="card-button secondary" @click="closeModal">Cancel</button>
                <button class="card-button primary" @click="deleteItem">Delete</button>
            </footer>
        </div>
    </div>
</template>

<style>
.delete-card {
    position: absolute;
    /* Cambiado de fixed a absolute */
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    min-width: 400px;
    padding: 30px;
    padding-bottom: 15px;
    border-radius: 20px;
    background-color: #111827;
    box-shadow: 20px 20px 30px rgba(0, 0, 0, 0.068);
}

.card-content {
    display: flex;
    flex-direction: column;
    height: fit-content;
}

.card-heading {
    font-size: 20px;
    font-weight: 700;
    color: #edf0f1;
    margin-bottom: 0.2rem;
}

.card-description {
    font-weight: 100;
    color: rgb(162, 162, 162);
}

.card-button-wrapper {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 10px;
}

.card-button {
    width: 50%;
    height: 35px;
    border-radius: 10px;
    border: none;
    cursor: pointer;
    font-weight: 600;
}

.primary {
    background-color: #dc3545;
    color: white;
}

.primary:hover {
    background-color: #b02a37;
}

.secondary {
    background-color: #ddd;
}

.secondary:hover {
    background-color: rgb(197, 197, 197);
}
</style>