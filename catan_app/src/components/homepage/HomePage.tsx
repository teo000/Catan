import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import useFetch from "../../hooks/useFetch";
import {LobbyPlayerResponse} from "../../responses/LobbyPlayerResponse";
import './homepage.css';

const HomePage = () => {
    const { data, error, loading, request } = useFetch<LobbyPlayerResponse>('/api/v1/Lobby');

    const navigate = useNavigate();
    const [lobbyCode, setLobbyCode] = useState('');
    const [playerName, setPlayerName] = useState('');

    const createLobby = async () => {
        const requestData = { playerName };

        try {
            const response = await request('/create', 'post', requestData);
            if (response && response.lobby && response.lobby.joinCode) {
                navigate(`/lobby/${response.lobby.joinCode}`);
            } else {
                console.error('Failed to create lobby: Invalid response format', response);
            }
        } catch (err) {
            console.error('Failed to create lobby', err);
        }
    };

    const joinLobby = async () => {
        const requestData = { lobbyCode, playerName };

        try {
            const response = await request('/join', 'post', requestData);
            if (response && response.success && response.success) {
                navigate(`/lobby/${lobbyCode}`);
            } else {
                console.error('Failed to create lobby: Invalid response format', response);
            }
        } catch (err) {
            console.error('Failed to create lobby', err);
        }

    };

    return (
        <div className="homepage" >
            <h1>Welcome to Settlers of Catan</h1>
            <button onClick={createLobby}>Create Lobby</button>
            <input
                type="text"
                value={lobbyCode}
                onChange={(e) => setLobbyCode(e.target.value)}
                placeholder="Enter Lobby Code"
            />
            <input
                type="text"
                value={playerName}
                onChange={(e) => setPlayerName(e.target.value)}
                placeholder="Enter your name"
            />
            <button onClick={joinLobby}>Join Lobby</button>
        </div>
    );
};

export default HomePage;