import React from 'react';
import './App.css';
import './components/settlements/settlements.css';
import './components/roads/roads.css';
import './components/dice/dice.css';
import './components/hexTiles/hexTiles.css';
import {GameLayout} from "./components/GameLayout";

function App() {
  return (
      <div className="app">
        <GameLayout/>
      </div>
  );
}

export default App;
