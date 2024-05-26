import {Lobby} from "../interfaces/Lobby";

export interface LobbyResponse {
    lobby: Lobby;
    success: boolean;
    message: string;
    validationErrors: any | null;
}