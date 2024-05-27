import {Player} from "./Player";
import {Map} from "./Map"

export interface GameSession {
    id: string;
    map: Map;
    players: Player[];
    turnPlayer: Player;
    gameStatus: string;
    turnEndTime: string;
    round: number;
    trades: Record<string, any>;
}
