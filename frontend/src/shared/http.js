import axios from "axios";
import { reactive } from "vue";

const API_URL = import.meta.env.VITE_API_URL;

const getHeaders = () => {
    const token = localStorage.getItem("token");
    return token ? { Authorization: `Bearer ${token}` } : {};
};

export const modals = reactive({
    errorModal: false,
    successModal: false,
    message: ""
});

const handleResponse = (response) => {
    modals.message = response.data ?? "";
    modals.successModal = true;
};

const handleError = (error) => {
    if (error.response?.status === 401) {
        localStorage.removeItem("token");
        if (window.updateAuthState) {
            window.updateAuthState();
        }
        modals.message = "Session expired. Please log in again.";
        modals.errorModal = true;
        return;
    }
    modals.message = "An unexpected error occurred";
    if (error.response?.data) {
        const data = error.response.data;
        modals.message = data;
        if (data.errors !== undefined) {
            modals.message = Object.entries(data.errors)
                // eslint-disable-next-line no-unused-vars
                .map(([key, value]) => `${Array.isArray(value) ? value.join(", ") : value}`)
                .join("\n");
        }
    }
    modals.errorModal = true;
};

export default {
    async get(url) {
        return await axios.get(`${API_URL}${url}`, { headers: getHeaders() })
            .catch((error) => handleError(error));
    },
    async post(url, data) {
        return await axios.post(`${API_URL}${url}`, data, { headers: getHeaders() })
            .then((response) => handleResponse(response))
            .catch((error) => handleError(error));
    },
    async put(url, data) {
        return await axios.put(`${API_URL}${url}`, data, { headers: getHeaders() })
            .then((response) => handleResponse(response))
            .catch((error) => handleError(error));
    },
    async delete(url, data) {
        return await axios.delete(`${API_URL}${url}/${data}`, { headers: getHeaders() })
            .catch((error) => handleError(error));
    },
    async login(data) {
        return await axios.post(`${API_URL}login`, data)
            .catch((error) => handleError(error));
    },
    async logout() {
        return await axios.post(`${API_URL}logout`, {}, { headers: getHeaders() })
            .catch((error) => handleError(error));
    }
}