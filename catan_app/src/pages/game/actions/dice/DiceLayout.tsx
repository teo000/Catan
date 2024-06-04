import {Dice} from "./Dice";
import {useCallback, useEffect, useState} from "react";
import useFetch from "../../../../hooks/useFetch";
import {usePlayer} from "../../../../context/PlayerProvider";
import {DiceRollResponse} from "../../../../responses/DiceRollResponse";
import {DiceRollDto} from "../../../../interfaces/DiceRollDto";
import {PlayerDto} from "../../../../interfaces/PlayerDto";

interface DiceLayoutProps {
    gameSessionId: string;
    diceRoll: DiceRollDto;
    turnPlayer: PlayerDto;
}


const DiceLayout: React.FC<DiceLayoutProps> = ({ gameSessionId, diceRoll , turnPlayer}) => {
    const { request } = useFetch<DiceRollResponse>('/api/v1/Game/roll-dice');
    const [diceNumbers, setDiceNumbers]
        = useState<(number | null)[]>([null, null]);
    const { player} = usePlayer();
    const [isRolling, setIsRolling] = useState(false);
    const firstDiceValue = diceRoll.values[0];
    const secondDiceValue = diceRoll.values[1];

    const isMyTurn = (turnPlayer.id === player?.id);

    const randomizeDice = useCallback(async () => {
        const requestData = { gameId: gameSessionId, playerId: player?.id };
        setIsRolling(true);
        const rollInterval = setInterval(() => {
            const newDiceNumbers = [
                Math.floor(Math.random() * 6) + 1,
                Math.floor(Math.random() * 6) + 1,
            ];
            setDiceNumbers(newDiceNumbers);
        }, 50);
        try {
            const response = await request('', 'post', requestData);
            if (response === null || !response.success || !response.diceRoll) {
                console.error('Failed to roll dice: Invalid response format', response);
                clearInterval(rollInterval);
                setIsRolling(false);
                setDiceNumbers([null, null]);
                return;
            }
            setTimeout(() => {
                console.log(response.diceRoll);
                clearInterval(rollInterval);
                setDiceNumbers(response.diceRoll.values);
                setIsRolling(false);
            }, 1000);
        } catch (err) {
            console.error('Failed to roll dice', err);
            clearInterval(rollInterval);
            setIsRolling(false);
            setDiceNumbers([null, null]);
        }
    }, [gameSessionId, player, request]);

    useEffect(() => {
        if (diceRoll.values[0] !== diceNumbers[0] || diceRoll.values[1] !== diceNumbers[1])
            if(diceRoll.values[0] === 0 && diceRoll.values[1] === 0){
                setDiceNumbers([null, null]);
            } else {
            setIsRolling(true);
            const rollInterval = setInterval(() => {
                const newDiceNumbers = [
                    Math.floor(Math.random() * 6) + 1,
                    Math.floor(Math.random() * 6) + 1,
                ];
                setDiceNumbers(newDiceNumbers);
            }, 50);

            setTimeout(() => {
                clearInterval(rollInterval);
                setDiceNumbers(diceRoll.values);
                setIsRolling(false);
            }, 1000);
        }
    }, [firstDiceValue, secondDiceValue]);

    return (
        <div className="dice-layout">
            <div className="dice-container">
                <Dice number={diceNumbers[0]} />
                <Dice number={diceNumbers[1]} />
            </div>
            <button
                type="button"
                className="btn-roll-dice"
                onClick={randomizeDice}
                disabled={isRolling || !isMyTurn || diceRoll.rolledThisTurn}
            >
                Roll dice
            </button>
        </div>
    );
};

export default DiceLayout;

