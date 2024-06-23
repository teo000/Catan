import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'https://localhost:7251',
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true,
    timeout: 100000
});

export default apiClient;