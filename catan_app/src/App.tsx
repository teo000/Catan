import React from 'react';
import './App.css';
import './components/settlements/settlements.css';
import './components/roads/roads.css';
import './components/dice/dice.css';
import './components/hexTiles/hexTiles.css';
import './components/actionButton/action-button.css'
import {GameLayout} from "./components/GameLayout";
import {Route, Routes } from 'react-router-dom';
import HomePage from "./components/HomePage";


function App() {
    return (
        <div className="app">
            <Routes>
                <Route path="" element={<HomePage/>} />
                <Route path="/lobby/:code" element={<GameLayout/>} />
            </Routes>
        </div>
    );
}
export default App;
