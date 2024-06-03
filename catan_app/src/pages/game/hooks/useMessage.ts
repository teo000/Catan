import {GameSessionDto} from "../../../interfaces/GameSessionDto";
import {usePlayer} from "../../../context/PlayerProvider";

export const useMessage = (gameSession : GameSessionDto) => {
    const {player} = usePlayer()

    if (player?.id !== gameSession.turnPlayer.id)
        return "";

    if (gameSession.round === 1 || gameSession.round === 2){
        if (gameSession.turnPlayer.settlements.length > gameSession.turnPlayer.roads.length)
            return "Place a road";
        if (gameSession.turnPlayer.settlements.length < gameSession.round)
            return "Place a settlement";
        return ""
    }

    if (!gameSession.dice.rolledThisTurn)
        return "Roll the dice";

    return "";
}
