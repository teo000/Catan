import React, {useState, useEffect, createContext, ReactNode, useContext} from 'react';
import { PlayerDto } from '../interfaces/PlayerDto';

interface PlayerContextProps {
    player: PlayerDto | null;
    setPlayer: React.Dispatch<React.SetStateAction<PlayerDto | null>>;
    gameId: string | null;
    setGameId: React.Dispatch<React.SetStateAction<string | null>>;
}

export const PlayerContext = createContext<PlayerContextProps | undefined>(undefined);

export const PlayerProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [player, setPlayer] = useState<PlayerDto | null>(null);
    const [gameId, setGameId] = useState<string | null>(null);

    useEffect(() => {
        const storedPlayer = sessionStorage.getItem('player');
        const storedGameId = sessionStorage.getItem('gameId');

        if (storedPlayer) {
            setPlayer(JSON.parse(storedPlayer));
        }

        if (storedGameId) {
            setGameId(storedGameId);
        }
    }, []);

    useEffect(() => {
        // Save player to sessionStorage whenever it changes
        if (player) {
            sessionStorage.setItem('player', JSON.stringify(player));
        } else {
            sessionStorage.removeItem('player');
        }
    }, [player]);

    useEffect(() => {
        // Save gameId to sessionStorage whenever it changes
        if (gameId) {
            sessionStorage.setItem('gameId', gameId);
        } else {
            sessionStorage.removeItem('gameId');
        }
    }, [gameId]);

    return (
        <PlayerContext.Provider value={{ player, setPlayer, gameId, setGameId }}>
            {children}
        </PlayerContext.Provider>
    );
};

export const usePlayer = () => {
    const context = useContext(PlayerContext);
    if (!context) {
        throw new Error('usePlayer must be used within a PlayerProvider');
    }
    return context;
};