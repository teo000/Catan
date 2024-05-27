import useFetch from "../../hooks/useFetch";
import {LobbyResponse} from "../../responses/LobbyResponse";
import {usePlayer} from "../PlayerProvider";
import {GameSessionResponse} from "../../responses/GameSessionResponse";

export function EndTurnButton(){
    const { data, error, loading, request } = useFetch<GameSessionResponse>('/api/v1/Game');
    const {player, gameId} = usePlayer();


    async function onClick(){
        const requestData = {gameId: gameId, playerId: player?.id};

        console.log(requestData);

        try {
            const response = await request('/end-turn', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to end turn: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to end turn', err);
        }
    }

    return (
        <button
            className="end-turn-button"
            onClick={onClick}
        >
            End Turn
        </button>
    )
}