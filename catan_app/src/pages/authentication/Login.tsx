import React, { useState } from 'react';
import {useAuth} from "../../contexts/AuthContext";
import {useNavigate} from "react-router-dom";

const Login: React.FC = () => {
    const [username, setUsername] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const {currentUser, login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await login(username, password);
            navigate('/home');
            console.log(currentUser);
        } catch (error) {
            alert("Login failed!");
        }
    };

    return (
        <form className="auth-form" onSubmit={handleSubmit}>
            <h1>Login</h1>
            <div className="inputs">
                <div>
                    <label>Username:</label>
                    <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
                </div>
                <div>
                    <label>Password:</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
            </div>
            <button type="submit">Login</button>
        </form>
    );
};

export default Login;