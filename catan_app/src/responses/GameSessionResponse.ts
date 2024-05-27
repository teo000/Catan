import {GameSession} from "../interfaces/GameSession";

export interface LobbyResponse {
    gameSession: GameSession;
    success: boolean;
    message: string;
    validationErrors: any | null;
}