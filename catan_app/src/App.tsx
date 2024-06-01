import React from 'react';
import './App.css';
import {Route, Routes } from 'react-router-dom';
import HomePage from "./pages/homepage/HomePage";
import {Lobby} from "./pages/lobby/components/Lobby";


function App() {
    return (
        <div className="app">
            <Routes>
                <Route path="" element={<HomePage/>} />
                <Route path="/lobby/:code" element={<Lobby/>} />
            </Routes>
        </div>
    );
}
export default App;
