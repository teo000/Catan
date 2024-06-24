import useFetch from "../../../../hooks/useFetch";
import {usePlayer} from "../../../../contexts/PlayerProvider";
import {GameSessionResponse} from "../../../../responses/GameSessionResponse";

export function EndTurnButton({disabled} : {disabled:boolean}){
    const { request } = useFetch<GameSessionResponse>('/api/v1/Game');
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
            className={`end-turn-button ${disabled ? 'disabled' : ''} `}
            onClick={onClick}
        >
            End Turn
        </button>
    )
}