import React from 'react';
import './App.css';
import {Route, Routes } from 'react-router-dom';
import HomePage from "./pages/homepage/HomePage";
import {Lobby} from "./pages/lobby/components/Lobby";
import {Authentication} from "./pages/authentication/Authentication";
import PrivateRoute from "./misc/PrivateRoute";


function App() {
    return (
        <div className="app">
            <Routes>
                <Route path="/" element={<Authentication />} />
                <Route path="/home" element={<PrivateRoute element={<HomePage />} />} />
                <Route path="/lobby/:code" element={<PrivateRoute element={<Lobby />} />} />
            </Routes>
        </div>
    );
}
export default App;
