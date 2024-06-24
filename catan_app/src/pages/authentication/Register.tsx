// src/components/Register.tsx
import React, { useState } from 'react';
import {useAuth} from "../../contexts/AuthContext";
import {useNavigate} from "react-router-dom";

const Register: React.FC = () => {
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const { register } = useAuth();
    const navigate = useNavigate();


    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await register(email, username, password);
            navigate('/home');
        } catch (error) {
            alert(`Registration failed!: ${error}`);
        }
    };

    return (
        <form className="auth-form" onSubmit={handleSubmit}>
            <h2>Register</h2>
            <div className="inputs">
            <div className= "input-label-div" >
                <label>Username:</label>
                <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
            </div>
            <div className= "input-label-div" >
                <label>Password:</label>
                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div className= "input-label-div" >
                <label>Email:</label>
                <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
            </div>
            </div>
            <button type="submit">Register</button>
        </form>
    );
};

export default Register;
