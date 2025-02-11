import axios from "axios";

const API_URL = import.meta.env.VITE_API_URL;
const token = localStorage.getItem("token");

export default {
    async get(url) {
        return await axios.get(`${API_URL}${url}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    async post(url, data) {
        return await axios.post(`${API_URL}${url}`, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    async put(url, data){
        return await axios.put(`${API_URL}${url}`, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    },
    async delete(url, data) {
        return await axios.delete(`${API_URL}${url}/${data}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}