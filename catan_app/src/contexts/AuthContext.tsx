import React, { createContext, useState, useEffect, ReactNode, useContext } from 'react';
import authService from '../services/authService';

interface AuthContextType {
    currentUser: string | null;
    login: (username: string, password: string) => Promise<void>;
    logout: () => void;
    register: (email: string, username: string, password: string) => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [currentUser, setCurrentUser] = useState<string | null>(authService.getCurrentUsername());

    const login = async (username: string, password: string) => {
        const token = await authService.login(username, password);
        if (token) {
            setCurrentUser(username);
        }
    };

    const logout = () => {
        authService.logout();
        setCurrentUser(null);
    };

    const register = async (email: string, username: string, password: string) => {
        await authService.register(email, username, password);
        const token = await authService.login(username, password);
        if (token) {
            setCurrentUser(username);
        }
    };

    useEffect(() => {
        setCurrentUser(authService.getCurrentUsername());
    }, []);

    return (
        <AuthContext.Provider value={{ currentUser, login, logout, register }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};