import {GameSessionDto} from "../interfaces/GameSessionDto";

export interface GameSessionResponse {
    gameSession: GameSessionDto;
    playerId: string;
    success: boolean;
    message: string;
    validationErrors: any | null;
}