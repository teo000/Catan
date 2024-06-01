import {GameSessionDto} from "../interfaces/GameSessionDto";

export interface GameSessionResponse {
    gameSession: GameSessionDto;
    success: boolean;
    message: string;
    validationErrors: any | null;
}