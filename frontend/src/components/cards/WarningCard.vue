<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import { onClickOutside } from '@vueuse/core';

const warningModalRef = ref(null);
const emits = defineEmits(["update:modelValue", "confirmAction"]);
const props = defineProps({
    modelValue: {
        type: Boolean,
        required: true
    },
    title: {
        type: String,
        required: true
    },
    message: {
        type: String,
        required: true
    },
    showActions: {
        type: Boolean,
        default: true
    }
});

onMounted(() => {
    document.addEventListener("keyup", handleClose);
});
onUnmounted(() => {
    document.removeEventListener("keyup", handleClose);
});

const closeModal = () => {
    emits("update:modelValue", false);
};

const confirmAction = () => {
    emits("update:modelValue", false);
    emits("confirmAction");
};

onClickOutside(warningModalRef, closeModal);

const handleClose = (event) => {
    if (event.key === 'Escape') {
        closeModal();
    }
};
</script>

<template>
    <div v-if="modelValue" class="warning-notifications-container" ref="warningModalRef">
        <div class="warning">
            <div class="warning-flex">
                <div class="warning-flex-shrink-0">
                    <svg aria-hidden="true" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"
                        class="warning-svg">
                        <path clip-rule="evenodd"
                            d="M8.485 2.495c.673-1.167 2.357-1.167 3.03 0l6.28 10.875c.673 1.167-.17 2.625-1.516 2.625H3.72c-1.347 0-2.19-1.458-1.515-2.625L8.485 2.495zM10 5a.75.75 0 01.75.75v3.5a.75.75 0 01-1.5 0v-3.5A.75.75 0 0110 5zm0 9a1 1 0 100-2 1 1 0 000 2z"
                            fill-rule="evenodd"></path>
                    </svg>
                </div>
                <div class="warning-prompt-wrap">
                    <p class="warning-prompt-heading">{{ props.title }}</p>
                    <p class="warning-prompt-text">{{ props.message }}</p>
                    <div class="warning-button-container" v-if="showActions">
                        <button class="warning-button-main" type="button" @click="confirmAction">
                            Continue
                        </button>
                        <button @click="closeModal" class="warning-button-secondary" type="button">
                            Cancel
                        </button>
                    </div>
                    <div v-else class="warning-button-container">
                        <button @click="closeModal" class="warning-button-main" type="button">
                            Got it!
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.warning-notifications-container {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    min-width: 380px;
    height: auto;
    font-size: 0.875rem;
    line-height: 1.25rem;
    display: flex;
    flex-direction: column;
    gap: 1rem;
    z-index: 1000;
}

.warning-flex {
    display: flex;
}

.warning-flex-shrink-0 {
    flex-shrink: 0;
}

.warning {
    padding: 1rem;
    border-radius: 0.375rem;
    background-color: rgb(254 252 232);
    border: 1px solid rgb(251 191 36);
}

.warning-svg {
    color: rgb(245 158 11);
    width: 1.25rem;
    height: 1.25rem;
}

.warning-prompt-wrap {
    margin-left: 0.75rem;
}

.warning-prompt-heading {
    font-weight: bold;
    color: rgb(146 64 14);
    margin-bottom: 0.5rem;
}

.warning-prompt-text {
    margin-top: 0.5rem;
    color: rgb(180 83 9);
    line-height: 1.4;
}

.warning-button-container {
    display: flex;
    margin-top: 0.875rem;
    margin-bottom: -0.375rem;
    margin-left: -0.5rem;
    margin-right: -0.5rem;
    gap: 0.5rem;
}

.warning-button-main {
    padding-top: 0.375rem;
    padding-bottom: 0.375rem;
    padding-left: 0.75rem;
    padding-right: 0.75rem;
    background-color: rgb(245 158 11);
    color: white;
    font-size: 0.875rem;
    line-height: 1.25rem;
    font-weight: 600;
    border-radius: 0.375rem;
    border: none;
    cursor: pointer;
    transition: background-color 0.2s;
}

.warning-button-main:hover {
    background-color: rgb(217 119 6);
}

.warning-button-secondary {
    padding-top: 0.375rem;
    padding-bottom: 0.375rem;
    padding-left: 0.75rem;
    padding-right: 0.75rem;
    background-color: transparent;
    color: rgb(146 64 14);
    font-size: 0.875rem;
    line-height: 1.25rem;
    border-radius: 0.375rem;
    border: 1px solid rgb(217 119 6);
    cursor: pointer;
    transition: all 0.2s;
}

.warning-button-secondary:hover {
    background-color: rgb(254 243 199);
}
</style>
