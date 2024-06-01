import {LobbyDto} from "../interfaces/LobbyDto";

export interface LobbyResponse {
    lobby: LobbyDto;
    success: boolean;
    message: string;
    validationErrors: any | null;
}