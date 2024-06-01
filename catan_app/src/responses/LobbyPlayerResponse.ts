import {LobbyDto} from "../interfaces/LobbyDto";

export interface LobbyPlayerResponse {
    lobby : LobbyDto;
    playerId: string;
    success: boolean;
    message: string;
    validationErrors: string[] | null;
}