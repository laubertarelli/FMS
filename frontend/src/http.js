import state from "./state";
import axios from "axios";

const API_URL = "https://localhost:5000/"; 

export default {
    async get(url) {
        return await axios.get(`${API_URL}${url}`, {
            headers: {
                Authorization: `Bearer ${state.token}`
            }
        });
    },
    async post(url, data) {
        return await axios.post(`${API_URL}${url}`, data, {
            headers: {
                Authorization: `Bearer ${state.token}`
            }
        });
    }
}