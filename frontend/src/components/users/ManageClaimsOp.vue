<script setup>
import http from "@/shared/http";
import { onMounted, reactive, ref, computed} from "vue";
import { useRoute, useRouter } from "vue-router";

const route = useRoute();
const router = useRouter();
const allClaims = reactive([]);
const userClaims = reactive([]);
const originalUserClaims = reactive([]);
const isLoading = ref(false);
const isSaving = ref(false);
const error = ref('');
const successMessage = ref('');

// Computed para verificar si hay cambios pendientes
const hasChanges = computed(() => {
    const currentClaimValues = userClaims.map(c => c.value).sort();
    const originalClaimValues = originalUserClaims.map(c => c.value).sort();
    return JSON.stringify(currentClaimValues) !== JSON.stringify(originalClaimValues);
});

// Computed para generar la lista de claims con su estado
const claimsWithState = computed(() => {
    return allClaims.map(claim => ({
        ...claim,
        isSelected: userClaims.some(userClaim => userClaim.value === claim.name.toLowerCase())
    }));
});

onMounted(async () => {
    await loadData();
});

async function loadData() {
    try {
        isLoading.value = true;
        error.value = '';
        
        // Validar que tenemos un userId en la URL
        if (!route.params.id) {
            error.value = 'User ID is required in URL';
            return;
        }
        
        // Cargar todos los claims disponibles
        const claimsResult = await http.get("users/claims");
        allClaims.splice(0, allClaims.length, ...claimsResult.data);

        // Cargar claims del usuario
        const userClaimsResult = await http.get(`users/claims/${route.params.id}`);
        const userClaimsData = userClaimsResult.data.map(claim => ({ 
            value: claim.value || claim.type || claim.name?.toLowerCase() 
        }));
        
        userClaims.splice(0, userClaims.length, ...userClaimsData);
        originalUserClaims.splice(0, originalUserClaims.length, ...userClaimsData);
        
    } catch {
        error.value = 'Error loading user permissions. Redirecting...';
        setTimeout(() => {
            router.replace({ name: "Users", params: { id: 1 } });
        }, 2000);
    } finally {
        isLoading.value = false;
    }
}

function toggleClaim(claimName) {
    const claimValue = claimName.toLowerCase();
    const existingIndex = userClaims.findIndex(c => c.value === claimValue);
    
    if (existingIndex !== -1) {
        // Remover claim
        userClaims.splice(existingIndex, 1);
    } else {
        // Agregar claim
        userClaims.push({ value: claimValue });
    }
    
    // Limpiar mensajes
    error.value = '';
    successMessage.value = '';
}

async function saveChanges() {
    try {
        isSaving.value = true;
        error.value = '';
        successMessage.value = '';
        
        // Preparar los claims en el formato que espera el backend
        const claimsToSend = allClaims.map(claim => ({
            claimValue: claim.name.toLowerCase(),
            isSelected: userClaims.some(userClaim => userClaim.value === claim.name.toLowerCase())
        }));

        await http.put("users/claims", { 
            userId: route.params.id, 
            claims: claimsToSend 
        });
        
        // Actualizar los claims originales para reflejar el nuevo estado
        originalUserClaims.splice(0, originalUserClaims.length, ...userClaims);
        
        successMessage.value = 'Permissions updated successfully!';
        
        // Limpiar mensaje de éxito después de 3 segundos
        setTimeout(() => {
            successMessage.value = '';
        }, 3000);
        
    } catch (err) {
        error.value = err.response?.data?.message || 'Error updating permissions. Please try again.';
    } finally {
        isSaving.value = false;
    }
}

function resetChanges() {
    userClaims.splice(0, userClaims.length, ...originalUserClaims);
    error.value = '';
    successMessage.value = '';
}
</script>

<template>
    <div class="div-container">
        <h2 id="heading">User Permissions</h2>
        
        <!-- Loading State -->
        <div v-if="isLoading" class="loading-container">
            <p class="text-white">Loading permissions...</p>
        </div>
        
        <!-- Error Message -->
        <div v-if="error" class="alert alert-error">
            {{ error }}
        </div>
        
        <!-- Success Message -->
        <div v-if="successMessage" class="alert alert-success">
            {{ successMessage }}
        </div>
        
        <!-- Claims Table -->
        <div v-if="!isLoading">
            <table class="table text-white p-2">
                <thead>
                    <tr>
                        <th>Permission</th>
                        <th>Status</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="claim in claimsWithState" :key="claim.value">
                        <td>{{ claim.name }}</td>
                        <td>
                            <span :class="claim.isSelected ? 'status-granted' : 'status-denied'">
                                {{ claim.isSelected ? 'Granted' : 'Denied' }}
                            </span>
                        </td>
                        <td>
                            <button 
                                :class="claim.isSelected ? 'btn btn-danger width' : 'btn btn-success width'"
                                @click="toggleClaim(claim.name)"
                                :disabled="isSaving">
                                {{ claim.isSelected ? 'Remove' : 'Grant' }}
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
            
            <!-- Action Buttons -->
            <div class="div-btn">
                <button 
                    v-if="hasChanges" 
                    @click="resetChanges" 
                    class="form-btn grey"
                    :disabled="isSaving">
                    Reset Changes
                </button>
                <button 
                    v-if="hasChanges"
                    @click="saveChanges" 
                    class="form-btn"
                    :disabled="isSaving">
                    {{ isSaving ? 'Saving...' : 'Save Changes' }}
                </button>
                <div v-if="!hasChanges" class="no-changes">
                    <p class="text-muted">No changes to save</p>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.div-container {
    background-color: #1f2937;
    width: 35%;
    padding: 1.3rem 2rem;
    border-radius: 10px;
    min-width: 500px;
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

.loading-container {
    text-align: center;
    padding: 2rem;
}

.alert {
    padding: 0.75rem 1rem;
    margin-bottom: 1rem;
    border-radius: 0.375rem;
    font-weight: 500;
}

.alert-error {
    background-color: #fef2f2;
    color: #dc2626;
    border: 1px solid #fecaca;
}

.alert-success {
    background-color: #f0fdf4;
    color: #16a34a;
    border: 1px solid #bbf7d0;
}

.status-granted {
    color: #16a34a;
    font-weight: 600;
}

.status-denied {
    color: #dc2626;
    font-weight: 600;
}

.div-btn {
    display: flex;
    gap: 1rem;
    justify-content: center;
    margin-top: 1.5rem;
}

.form-btn:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.no-changes {
    text-align: center;
    width: 100%;
}

.text-muted {
    color: #9ca3af;
    font-style: italic;
    margin: 0;
}

.table th {
    font-weight: 600;
    border-bottom: 2px solid #374151;
}

.table td {
    padding: 0.75rem;
    border-bottom: 1px solid #374151;
}
</style>