import {GameSessionDto} from "./GameSessionDto";
import {PlayerDto} from "./PlayerDto";

export interface LobbyDto {
    id: string;
    joinCode: string;
    players: PlayerDto[];
    gameSession: GameSessionDto | null;
}