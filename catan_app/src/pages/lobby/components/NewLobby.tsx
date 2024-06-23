import React, { useEffect, useState, useCallback } from 'react';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import { useLocation } from 'react-router-dom';
import { WaitingRoom } from './WaitingRoom';
import useFetch from "../../../hooks/useFetch";
import {usePlayer} from "../../../context/PlayerProvider";
import {GameSessionDto} from "../../../interfaces/GameSessionDto";
import {LobbyResponse} from "../../../responses/LobbyResponse";
import {PlayerDto} from "../../../interfaces/PlayerDto";
import {GameLayout} from "../../game/GameLayout";


export const NewLobby = React.memo(() => {
    const { data, error, loading, request } = useFetch<LobbyResponse>('/api/v1/NewLobby');

    const location = useLocation();
    const currentPath = location.pathname;
    const joinCode = currentPath.substring(currentPath.lastIndexOf('/') + 1);

    const [players, setPlayers] = useState<PlayerDto[]>([]);
    const { player, setGameId } = usePlayer();
    const [loaded, setLoaded] = useState(false);

    const requestData = { joinCode };

    const gameStatus = data?.lobby.gameSession?.gameStatus;
    const isOver = gameStatus === 'Abandoned' || gameStatus === 'Finished';

    const onStartGame = useCallback(async () => {
        try {
            const response = await request('/start', 'post', requestData);
            if (response === null || !response.success) {
                console.error('Failed to create lobby: Invalid response format', response);
            }
        } catch (err) {
            console.error('Failed to create lobby', err);
        }
        setLoaded(false);
    }, [request]);

    useEffect(() => {
        const connection: HubConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7251/gameHub')
            .build();

        connection.on('ReceiveGameSession', (gameSession: GameSessionDto) => {
            console.log('Received game session:', gameSession);
            setGameId(gameSession.id);
        });

        connection.start()
            .then(() => connection.invoke('JoinGroup', joinCode))
            .catch(err => console.error('SignalR Connection Error:', err));

        return () => {
            connection.invoke('LeaveGroup', joinCode)
                .then(() => connection.stop())
                .catch(err => console.error('SignalR Disconnection Error:', err));
        };
    }, [joinCode, setGameId]);

    useEffect(() => {
        if (data?.lobby?.players) {
            setPlayers(data.lobby.players);
        }
    }, [data, setPlayers]);

    useEffect(() => {
        if (data?.lobby.gameSession) {
            setGameId(data.lobby.gameSession.id);
        }
    }, [data, setGameId]);

    useEffect(() => {
        if (!loading) {
            setLoaded(true);
        }
    }, [loading]);

    if (error) return <p>Error: {error}</p>;
    if (!loaded) return <div className="homepage-container"><p>Loading...</p></div>;

    if (data === undefined || data === null) {
        return <div className="waiting-room-container"><p>Loading...</p></div>;
    }

    const gameSession = data.lobby.gameSession;
    if (gameSession !== undefined && gameSession !== null) {
        return <GameLayout gameSession={gameSession} />;
    }

    const lobby = data.lobby;
    if (lobby !== undefined && lobby !== null) {
        return <WaitingRoom players={players} onClick={onStartGame} />;
    }

    return <p>idk at this point</p>;
});