import React, { createContext, useContext, useState, ReactNode } from 'react';
import {Player} from "../interfaces/Player";
interface PlayerContextType {
    player: Player | null;
    setPlayer: React.Dispatch<React.SetStateAction<Player | null>>;
}

const PlayerContext = createContext<PlayerContextType | undefined>(undefined);

export const PlayerProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [player, setPlayer] = useState<Player | null>(null);

    return (
        <PlayerContext.Provider value={{ player, setPlayer }}>
            {children}
        </PlayerContext.Provider>
    );
};

export const usePlayer = (): PlayerContextType => {
    const context = useContext(PlayerContext);
    if (context === undefined) {
        throw new Error('usePlayer must be used within a PlayerProvider');
    }
    return context;
};