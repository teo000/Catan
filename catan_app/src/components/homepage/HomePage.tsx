import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import useFetch from "../../hooks/useFetch";
import {LobbyPlayerResponse} from "../../responses/LobbyPlayerResponse";
import './homepage.css';
import {usePlayer} from "../PlayerProvider";

const HomePage = () => {
    const { data, error, loading, request } = useFetch<LobbyPlayerResponse>('/api/v1/Lobby');

    const navigate = useNavigate();
    const [lobbyCode, setLobbyCode] = useState('');
    const [playerName, setPlayerName] = useState('');

    const { player, setPlayer } = usePlayer();

    const createLobby = async () => {
        const requestData = { playerName };

        try {
            const response = await request('/create', 'post', requestData);
            if (response && response.lobby && response.lobby.joinCode) {
                if( !player && response.playerId && response.lobby.players){
                    const foundPlayer = response.lobby.players.find(player => player.id === response.playerId);
                    if (foundPlayer)
                        setPlayer(foundPlayer)
                    console.log(foundPlayer?.name);
                    navigate(`/lobby/${response.lobby.joinCode}`);
                }
            } else {
                console.error('Failed to create lobby: Invalid response format', response);
            }
        } catch (err) {
            console.error('Failed to create lobby', err);
        }
    };
    const joinLobby = async () => {
        const requestData = { joinCode : lobbyCode, playerName };

        try {
            const response = await request('/join', 'post', requestData);
            if (response && response.success && response.success) {
                const foundPlayer = response.lobby.players.find(player => player.id === response.playerId);
                if (foundPlayer)
                    setPlayer(foundPlayer)
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