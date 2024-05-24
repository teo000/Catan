import {Dice} from "./Dice";
import {useState} from "react";

function DiceLayout(){
    const [diceNumbers, setDiceNumbers] = useState([0, 0])

    function randomizeDice(){

        const rollInterval = setInterval(() => {
            const newDiceNumbers = [
                Math.floor(Math.random() * 6) + 1,
                Math.floor(Math.random() * 6) + 1
            ];
            setDiceNumbers(newDiceNumbers);
        }, 50);

        setTimeout(() => {
            clearInterval(rollInterval);
        }, 1000);
    }

    return (
        <div className="dice-layout">
            <div className="dice-container">
                <Dice number={diceNumbers[0]}/>
                <Dice number={diceNumbers[1]}/>
            </div>
            <button type="button" className="btn-roll-dice" onClick={randomizeDice}>Roll dice</button>
        </div>
    )
}

export {DiceLayout};