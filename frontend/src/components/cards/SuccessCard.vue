<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import { onClickOutside } from '@vueuse/core';
import { useRouter } from 'vue-router';

const emits = defineEmits(["update:modelValue", "resetMessage"]);
const props = defineProps({
    modelValue: {
        type: Boolean,
        required: true
    },
    message: {
        type: String,
        required: true
    }
});

const type = props.message.match(/(?<=\s)\w+/)?.[0] ?? '';
const router = useRouter();
const successModalRef = ref(null);

onMounted(() => {
    document.addEventListener("keyup", handleClose);
});
onUnmounted(() => {
    document.removeEventListener("keyup", handleClose);
});

const closeAndReset = () => {
    emits("update:modelValue", false);
    emits("resetMessage");
};

const closeModal = () => {
    closeAndReset();
    window.location.reload();
};
onClickOutside(successModalRef, closeModal);

const handleClose = (event) => {
    if (event.key === 'Escape') {
        closeModal();
    }
};

function redirect() {
    if (type == "account") {
        router.replace("/account");
    } else {
        router.replace(`/${type}s/1`);
    }
    closeAndReset();
}
</script>

<template>
    <div class="notifications-container" ref="successModalRef">
        <div class="success">
            <div class="flex">
                <div class="flex-shrink-0">
                    <svg aria-hidden="true" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"
                        class="succes-svg">
                        <path clip-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                            fill-rule="evenodd"></path>
                    </svg>
                </div>
                <div class="success-prompt-wrap">
                    <p class="success-prompt-heading">{{ props.message }}</p>
                    <div class="success-button-container">
                        <button v-if="type != 'account' " class="success-button-main" type="button" @click="redirect">
                           View all {{ type }}s
                        </button>
                        <button @click="closeModal" class="success-button-secondary" type="button">
                            Dismiss
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
.notifications-container {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    min-width: 320px;
    height: auto;
    font-size: 0.875rem;
    line-height: 1.25rem;
    display: flex;
    flex-direction: column;
    gap: 1rem;
}

.flex {
    display: flex;
}

.flex-shrink-0 {
    flex-shrink: 0;
}

.success {
    padding: 1rem;
    border-radius: 0.375rem;
    background-color: rgb(240 253 244);
}

.succes-svg {
    color: rgb(74 222 128);
    width: 1.25rem;
    height: 1.25rem;
}

.success-prompt-wrap {
    margin-left: 0.75rem;
}

.success-prompt-heading {
    font-weight: bold;
    color: rgb(22 101 52);
}

.success-prompt-prompt {
    margin-top: 0.5rem;
    color: rgb(21 128 61);
}

.success-button-container {
    display: flex;
    margin-top: 0.875rem;
    margin-bottom: -0.375rem;
    margin-left: -0.5rem;
    margin-right: -0.5rem;
}

.success-button-main {
    padding-top: 0.375rem;
    padding-bottom: 0.375rem;
    padding-left: 0.5rem;
    padding-right: 0.5rem;
    background-color: #ECFDF5;
    color: rgb(22 101 52);
    font-size: 0.875rem;
    line-height: 1.25rem;
    font-weight: bold;
    border-radius: 0.375rem;
    border: none
}

.success-button-main:hover {
    background-color: #D1FAE5;
}

.success-button-secondary {
    padding-top: 0.375rem;
    padding-bottom: 0.375rem;
    padding-right: 0.5rem;
    margin-left: 0.75rem;
    background-color: #ECFDF5;
    color: #065F46;
    font-size: 0.875rem;
    line-height: 1.25rem;
    border-radius: 0.375rem;
    border: none;
}
</style>