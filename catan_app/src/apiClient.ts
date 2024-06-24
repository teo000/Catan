import axios from 'axios';
import authService from "./services/authService";

const apiClient = axios.create({
    baseURL: "https://localhost:7251"
});

apiClient.interceptors.request.use(
    config => {
        const token = authService.getToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

export default apiClient;