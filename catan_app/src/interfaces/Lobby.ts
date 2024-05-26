import {GameSession} from "./GameSession";
import {Player} from "./Player";

export interface Lobby {
    id: string;
    joinCode: string;
    players: Player[];
    gameSession: GameSession | null;
}