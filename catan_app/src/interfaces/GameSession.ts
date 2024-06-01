import {Player} from "./Player";
import {Map} from "./Map"
import {DiceRoll} from "./DiceRoll";
import {TradeDto} from "./TradeDto";
import {LongestRoad} from "./LongestRoad";

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
    winner: Player | null;
    longestRoad: LongestRoad;
}
