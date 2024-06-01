import React from 'react';
import './App.css';
import './pages/game/board/settlements/settlements.css';
import './pages/game/board/roads/roads.css';
import './pages/game/actions/dice/dice.css';
import './pages/game/board/hexTiles/hexTiles.css';
import './pages/game/actions/actionButton/action-button.css'
import './pages/game/actions/turnTimerLabel/turn-timer-label.css'
import {GameLayout} from "./pages/game/GameLayout";
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
