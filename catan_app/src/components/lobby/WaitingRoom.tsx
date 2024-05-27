import React from "react";
import {Lobby} from "../../interfaces/Lobby";
import {Player} from "../../interfaces/Player";

interface WaitingRoomProps{
    players : Player[],
    onClick : () => void
}

export const WaitingRoom : React.FC<WaitingRoomProps> = ({players, onClick}) =>{

    return(
        <div className = "waiting-room">
            <div className="players-list">
                {players.map((player) => (
                    <div key={player.id} className="player">
                        {player.name}
                    </div>
                ))}
            </div>
            <button
                className="start-game-button"
                onClick={onClick}
            >
                Start Game
            </button>
        </div>
    );
}