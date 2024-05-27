import {Lobby} from "../interfaces/Lobby";

export interface LobbyPlayerResponse {
    lobby : Lobby;
    playerId: string;
    success: boolean;
    message: string;
    validationErrors: string[] | null;
}