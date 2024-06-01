import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import {BrowserRouter, Routes, Route} from "react-router-dom";
import {PlayerProvider} from "./context/PlayerProvider";

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

// root.render(
//   <React.StrictMode>
//     <App />
//   </React.StrictMode>
// );

// axios.defaults.baseURL = 'https://localhost:7251';

root.render(
    <BrowserRouter>
        <PlayerProvider>
            <App/>
        </PlayerProvider>
    </BrowserRouter>
    // document.getElementById('root')
)

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
