import {TradeDto} from "../../../../interfaces/TradeDto";
import {PlayerDto} from "../../../../interfaces/PlayerDto";
import React from "react";
import {TradeList} from "./TradesList";
import {PlayerInfoDiv} from "./PlayerInfoDiv";

interface ChatDivProps{
    trades: TradeDto[];
    players: PlayerDto[];
}

export const ChatDiv: React.FC<ChatDivProps> = ({trades, players}) => {
    return (
        <div className="chat-div">
            <TradeList trades={trades} players={players}/>
            <PlayerInfoDiv players={players} />
        </div>
    );
}