export interface LobbyPlayerResponse {
    lobby: {
        id: string;
        joinCode: string;
        players: Array<{
            id: string;
            name: string;
            isActive: boolean;
            resourceCount: Record<string, number>;
            settlements: any[];
            roads: any[];
        }>;
        gameSession: any | null;
    };
    playerId: string;
    success: boolean;
    message: string;
    validationErrors: string[] | null;
}