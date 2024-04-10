import React from 'react';
import logo from './logo.svg';
import './App.css';
import {GameMap} from "./components/GameMap";
import {GameLayout} from "./components/GameLayout";

function App() {
  return (
      <div className="app">
        <GameLayout/>
      </div>
  );
}

export default App;
