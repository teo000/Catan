import {GameMap} from "./GameMap";
import {ActionButton} from "./ActionButton";
import {Dice} from "./Dice";
import {DiceLayout} from "./DiceLayout";
import {SettlementSpots} from "./settlements/SettlementSpots";

function GameLayout(){
    return (
        <div className="gameLayout">
            <div className='board-div'>

                <img
                    className='board-background'
                    src='images/water_background.png'
                    alt='background'
                />

                <GameMap />
                <SettlementSpots/>
            </div>
            {/*<DiceLayout />*/}
            {/*<div className='action-bar'>*/}
            {/*    <ActionButton pieceType='road'></ActionButton>*/}
            {/*    <ActionButton pieceType='settlement'></ActionButton>*/}
            {/*    <ActionButton pieceType='city'></ActionButton>*/}
            {/*</div>*/}
        </div>
    );
}

export {GameLayout};