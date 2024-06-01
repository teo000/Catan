import {PlayerDto} from "./PlayerDto";
import {MapDto} from "./MapDto"
import {DiceRollDto} from "./DiceRollDto";
import {TradeDto} from "./TradeDto";
import {LongestRoadDto} from "./LongestRoadDto";

export interface GameSessionDto {
    id: string;
    map: MapDto;
    players: PlayerDto[];
    turnPlayer: PlayerDto;
    gameStatus: string;
    turnEndTime: string;
    round: number;
    trades: TradeDto[];
    dice: DiceRollDto;
    winner: PlayerDto | null;
    longestRoad: LongestRoadDto;
}
