import {DiceRollDto} from "../interfaces/DiceRollDto";

export interface DiceRollResponse {
    diceRoll: DiceRollDto;
    success: boolean;
    message: string;
    validationErrors: any | null;
}