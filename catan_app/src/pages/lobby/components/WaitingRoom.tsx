import React, {useState} from "react";
import {PlayerDto} from "../../../interfaces/PlayerDto";
import "./lobby.css"

interface Props {
    players: PlayerDto[];
    onClick: () => void;
}

export const WaitingRoom: React.FC<Props> = ({ players, onClick}) => {
    const [copySuccess, setCopySuccess] = useState(false);

    const currentPath = window.location.pathname;
    const joinCode = currentPath.substring(currentPath.lastIndexOf('/') + 1);

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
                <button className="copy-button" onClick={handleCopyJoinCode}>
                    {copySuccess ? 'Copied!' : 'Copy'}
                </button>
                <div className="players-list">
                    {players.map((player) => (
                        <div key={player.id} className="player">
                            {player.name}
                        </div>
                    ))}
                </div>


                <button className="start-game-button" onClick={onClick}>Start Game</button>
            </div>
        </div>
    );
};