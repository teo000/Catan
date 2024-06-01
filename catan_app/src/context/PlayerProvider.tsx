import React, { createContext, useContext, useState, ReactNode } from 'react';
import {PlayerDto} from "../interfaces/PlayerDto";
interface PlayerContextType {
    player: PlayerDto | null;
    setPlayer: React.Dispatch<React.SetStateAction<PlayerDto | null>>;
    gameId: string | null;
    setGameId: React.Dispatch<React.SetStateAction<string | null>>;
}

// Create the PlayerContext with the extended type
const PlayerContext = createContext<PlayerContextType | undefined>(undefined);

// Create the PlayerProvider component
export const PlayerProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [player, setPlayer] = useState<PlayerDto | null>(null);
    const [gameId, setGameId] = useState<string | null>(null);

    return (
        <PlayerContext.Provider value={{ player, setPlayer, gameId, setGameId }}>
            {children}
        </PlayerContext.Provider>
    );
};

// Custom hook to use the PlayerContext
export const usePlayer = (): PlayerContextType => {
    const context = useContext(PlayerContext);
    if (context === undefined) {
        throw new Error('usePlayer must be used within a PlayerProvider');
    }
    return context;
};