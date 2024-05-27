import React from 'react';
import './App.css';
import './components/settlements/settlements.css';
import './components/roads/roads.css';
import './components/dice/dice.css';
import './components/hexTiles/hexTiles.css';
import './components/actionButton/action-button.css'
import './components/turnTimerLabel/turn-timer-label.css'
import {GameLayout} from "./components/GameLayout";
import {Route, Routes } from 'react-router-dom';
import HomePage from "./components/homepage/HomePage";
import {Lobby} from "./components/lobby/Lobby";


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
