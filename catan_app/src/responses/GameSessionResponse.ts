import {GameSession} from "../interfaces/GameSession";

export interface GameSessionResponse {
    gameSession: GameSession;
    success: boolean;
    message: string;
    validationErrors: any | null;
}