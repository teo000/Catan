import {GameMap} from "./GameMap";
import {ActionButton} from "./ActionButton";
import {Dice} from "./Dice";
import {DiceLayout} from "./DiceLayout";

function GameLayout(){
    return (
        <div className="gameLayout">
            <GameMap />
            <DiceLayout />
            <div className='action-bar'>
                <ActionButton pieceType='road'></ActionButton>
                <ActionButton pieceType='settlement'></ActionButton>
                <ActionButton pieceType='city'></ActionButton>
            </div>
        </div>
    );
}

export {GameLayout};