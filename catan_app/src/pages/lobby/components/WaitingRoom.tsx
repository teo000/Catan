import React, {useState} from "react";
import {PlayerDto} from "../../../interfaces/PlayerDto";
import "./lobby.css"
import {usePlayer} from "../../../context/PlayerProvider";
import useFetch from "../../../hooks/useFetch";
import {BaseResponse} from "../../../responses/BaseResponse";

interface Props {
    players: PlayerDto[];
    onClick: () => void;
}

export const WaitingRoom: React.FC<Props> = ({ players, onClick}) => {
    const {request } = useFetch<BaseResponse>('/api/v1/Lobby');
    const [copySuccess, setCopySuccess] = useState(false);

    const currentPath = window.location.pathname;
    const joinCode = currentPath.substring(currentPath.lastIndexOf('/') + 1);
    const {player, gameId} = usePlayer();

    const addAIPlayer = async () => {
        const requestData = {
            joinCode
        };
        try {
            const response = await request('/add-ai', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to trade: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to trade bank', err);
        }
    };


    const handleCopyJoinCode = () => {
        navigator.clipboard.writeText(joinCode)
            .then(() => {
                setCopySuccess(true);
                setTimeout(() => setCopySuccess(false), 2000);
            })
            .catch(err => console.error('Failed to copy join code', err));
    };

    return (
        <div className="waiting-room-container">
            <div className="waiting-room">
                <label htmlFor="join-code" className="join-code-label">Join Code:</label>
                <input
                    id="join-code"
                    type="text"
                    value={joinCode}
                    readOnly
                />
                <button className="waiting-room-button" onClick={handleCopyJoinCode}>
                    {copySuccess ? 'Copied!' : 'Copy'}
                </button>
                <div className="players-list">
                    {players.map((player) => (
                        <div key={player.id} className="player">
                            {player.name}
                        </div>
                    ))}
                </div>

                <button className="waiting-room-button" onClick={addAIPlayer}>Add AI player</button>
                <button className="waiting-room-button" onClick={onClick}>Start Game</button>
            </div>
        </div>
    );
};