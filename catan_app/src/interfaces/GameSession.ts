import {Player} from "./Player";
import {Map} from "./Map"
import {DiceRoll} from "./DiceRoll";
import {TradeDto} from "./TradeDto";

export interface GameSession {
    id: string;
    map: Map;
    players: Player[];
    turnPlayer: Player;
    gameStatus: string;
    turnEndTime: string;
    round: number;
    trades: TradeDto[];
    dice: DiceRoll;
}
