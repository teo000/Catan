import {GameMap} from "./GameMap";
import {ActionButton} from "./ActionButton";

function GameLayout(){
    return (
        <div className="gameLayout">
            <GameMap />
            <div className='action-bar'>
                <ActionButton pieceType='road'></ActionButton>
                <ActionButton pieceType='settlement'></ActionButton>
                <ActionButton pieceType='city'></ActionButton>
            </div>
        </div>
    );
}

export {GameLayout};