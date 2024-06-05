import React, {useCallback, useEffect, useRef, useState} from "react";
import useFetch from "../../../hooks/useFetch";
import {LobbyResponse} from "../../../responses/LobbyResponse";
import {useLocation} from "react-router-dom";
import {GameLayout} from "../../game/GameLayout";
import {WaitingRoom} from "./WaitingRoom";
import {PlayerDto} from "../../../interfaces/PlayerDto";
import {useDeepCompareState} from "../../../hooks/useDeepCompareEffect";
import {usePlayer} from "../../../context/PlayerProvider";
import {MapLogicInfo} from "../../game/utils/MapLogicInfo";

export const Lobby = React.memo(() => {
    const { data, error, loading, request } = useFetch<LobbyResponse>('/api/v1/Lobby');

    const location = useLocation();
    const currentPath = location.pathname;
    const joinCode = currentPath.substring(currentPath.lastIndexOf('/') + 1);

    const [players, setPlayers] = useDeepCompareState<PlayerDto[]>([]);
    const {setGameId } = usePlayer();
    const [loaded, setLoaded] = useState(false);

    const requestData = {joinCode};

    const gameStatus = data?.lobby.gameSession?.gameStatus
    const isOver = gameStatus === 'Abandoned' || gameStatus === 'Finished'

    const onStartGame = useCallback(async () => {

        try {
            const response = await request('/start', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to create lobby: Invalid response format', response);
            }
        } catch (err) {
            console.error('Failed to create lobby', err);
        }
        setLoaded(false);

    }, [request]);

    const fetchGameState = useCallback(async () => {
        await request(`/${joinCode}`,'get');
    }, [request]);

    useEffect(() => {
        if (!isOver) {
            const intervalId = setInterval(fetchGameState, 333);
            return () => clearInterval(intervalId);
        }
    }, [fetchGameState, isOver]);



    useEffect(() => {
        if (data?.lobby?.players) {
            setPlayers(data.lobby.players);
        }
    }, [data, setPlayers]);

    useEffect(()=>{
        if (data?.lobby.gameSession)
            setGameId(data.lobby.gameSession.id)
    })

    useEffect(() =>{
        if (!loading)
            setLoaded(true);
    },[loading] )

    if (error) return <p>Error: {error}</p>;
    if (!loaded) return <p>Loading...</p>

    if (data === undefined || data === null)
        return <p> ... </p>

    const gameSession = data.lobby.gameSession;
    if (gameSession !== undefined && gameSession !== null) {
        return <GameLayout gameSession={gameSession}/>
    }

    const lobby = data.lobby;
    if (lobby !== undefined && lobby !== null)
        return <WaitingRoom players={players} onClick={onStartGame}/>

    return <p>
        idk at this point
    </p>

})