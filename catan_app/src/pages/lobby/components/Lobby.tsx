import useFetch from "../../../hooks/useFetch";
import {LobbyResponse} from "../../../responses/LobbyResponse";
import {useLocation} from "react-router-dom";
import {usePlayer} from "../../../contexts/PlayerProvider";
import React, {useCallback, useEffect, useState} from "react";
import {GameLayout} from "../../game/GameLayout";
import {WaitingRoom} from "./WaitingRoom";
import {HubConnection, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {LobbyDto} from "../../../interfaces/LobbyDto";
import {GameSessionDto} from "../../../interfaces/GameSessionDto";

export const Lobby = React.memo(() => {
    const { error, request } = useFetch<LobbyResponse>('/api/v1/Lobby');

    const location = useLocation();
    const currentPath = location.pathname;
    const joinCode = currentPath.substring(currentPath.lastIndexOf('/') + 1);

    const { player, setGameId } = usePlayer();
    const [loaded, setLoaded] = useState(false);
    const [lobby, setLobby] = useState<LobbyDto | null>(null);
    const [gameSession, setGameSession] = useState<GameSessionDto | null>(null);
    const [gameHasStarted, setGameHasStarted] = useState<boolean>(false);

    const requestData = { joinCode };

    const onStartGame = () => {
        async function fetchGame(){
            try {
                const response = await request('/start', 'post', requestData);
                if (response === null || !response.success) {
                    console.error('Failed to start game: Invalid response format', response);
                }
                else {
                    setLobby(response.lobby);
                    setGameSession(response.lobby.gameSession);
                    setGameHasStarted(true);
                }

            } catch (err) {
                console.error('Failed to start game', err);
            }
        }

        fetchGame();
    };

    useEffect(() => {
        async function fetchLobby() {
            const response = await request(`/${joinCode}?playerId=${player?.id}`, 'get');
            if (response && response.lobby) {
                setLobby(response.lobby);
                setLoaded(true);
            } else {
                console.error('Invalid lobby response:', response);
            }
        }

        fetchLobby();
    }, [player?.id, request, joinCode]);

    useEffect(() => {
        const connection: HubConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7251/lobbyHub')
            .configureLogging(LogLevel.Information)
            .build();

        connection.onclose((error) => {
            console.error('Connection closed: ', error);
        });

        connection.on('ReceiveLobby', (lobby: LobbyDto) => {
            console.log('Received lobby:', lobby);

            if (lobby) {
                setLoaded(true);
                setLobby(lobby);

                if (lobby.gameSession) {
                    setGameSession(lobby.gameSession)
                    setGameHasStarted(true);
                }
            }
        });

        connection.start()
            .then(() => {
                console.log('SignalR Connection Established');
                return connection.invoke('JoinGroup', joinCode);
            })
            .catch(err => console.error('SignalR Connection Error:', err));

        return () => {
            connection.invoke('LeaveGroup', joinCode)
                .then(() => connection.stop())
                .catch(err => console.error('SignalR Disconnection Error:', err));
        };

    }, [joinCode]);

    useEffect(() => {
        console.log(gameSession);

        if(gameSession) {
            const connection: HubConnection = new HubConnectionBuilder()
                .withUrl('https://localhost:7251/gameHub')
                .configureLogging(LogLevel.Information)
                .build();

            connection.onclose((error) => {
                console.error('Connection closed: ', error);
            });

            connection.onreconnecting((error) => {
                console.warn('Reconnecting: ', error);
            });

            connection.onreconnected((connectionId) => {
                console.log('Reconnected: ', connectionId);
            });

            connection.on('ReceiveGame', (game: GameSessionDto) => {
                console.log('Received game:', game);

                if (game)
                    setGameSession(game)

            });

            connection.start()
                .then(() => {
                    console.log('SignalR Connection Established');
                    return connection.invoke('JoinGroup', gameSession.id);
                })
                .catch(err => console.error('SignalR Connection Error:', err));

            return () => {
                connection.invoke('LeaveGroup', gameSession.id)
                    .then(() => connection.stop())
                    .catch(err => console.error('SignalR Disconnection Error:', err));
            };
        }
    }, );


    useEffect(() => {
        if (gameSession) {
            setGameId(gameSession.id);
        }
    }, [lobby?.gameSession, setGameId]);


    if (error) return <p>Error: {error}</p>;
    if (!loaded || !lobby) return <div className="waiting-room-container"> <p> Loading... </p> </div>;
    
    if (gameHasStarted && gameSession) {
        return <GameLayout gameSession={gameSession} />;
    } else {
        return <WaitingRoom players={lobby.players} onClick={onStartGame} />;
    }
});



















