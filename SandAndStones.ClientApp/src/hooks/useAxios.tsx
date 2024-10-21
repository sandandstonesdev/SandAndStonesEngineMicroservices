import axios from "axios";

export const axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_APP_BASE_URL,
    headers: {
        "Content-Type": "application/json",
        "Accept": 'application/json'
    }
});