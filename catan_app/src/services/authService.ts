// src/services/authService.ts
import axios from 'axios';

const API_URL = "https://localhost:7251/api/v1";

const register = (email:string, username: string, password: string) => {
    return axios.post(`${API_URL}/auth/register`, {
        email,
        username,
        password
    })
        .then(response => {
            // Handle successful response
            if (response.status === 201 && response.data) {
                console.log('Registration successful:', response.data);
                return response.data;
            }
        })
        .catch(error => {
            if (error.response) {
                console.log('Registration error:', error.response.data);
            } else if (error.request) {
                console.log('No response received:', error.request);
            } else {
                console.log('Error:', error.message);
            }
            throw error;
        });
};

const login = (username: string, password: string) => {
    return axios.post(`${API_URL}/auth/login`, {
        username,
        password
    }).then(response => {
        const token = response.data;
        if (token) {
            localStorage.setItem("token", token);
            localStorage.setItem("username", username);
        }
        return token;
    });
};

const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("username");
};

const getCurrentUsername = () => {
    const userStr = localStorage.getItem("username");
    return userStr ? userStr : null;
};

const getToken = () => {
    const token = localStorage.getItem("token");
    return token ? token : null;
};

export default {
    register,
    login,
    logout,
    getCurrentUsername,
    getToken
};
