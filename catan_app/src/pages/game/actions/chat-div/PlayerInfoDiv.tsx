import {PlayerDto} from "../../../../interfaces/PlayerDto";
import React from "react";
import {PlayerInfo} from "./PlayerInfo";

interface PlayerInfoDivProps {
    players: PlayerDto[];
}

export const PlayerInfoDiv: React.FC<PlayerInfoDivProps> = ({players}) => {
    return (
        <div className="player-info-div">
            {
                players.map(player =>
                    <PlayerInfo playerInfo={player}/>
                )
            }
        </div>
    )
};
