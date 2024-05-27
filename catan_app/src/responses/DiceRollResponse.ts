import {Lobby} from "../interfaces/Lobby";
import {DiceRoll} from "../interfaces/DiceRoll";

export interface DiceRollResponse {
    diceRoll: DiceRoll;
    success: boolean;
    message: string;
    validationErrors: any | null;
}