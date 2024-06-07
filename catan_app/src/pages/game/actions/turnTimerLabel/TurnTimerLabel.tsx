import React from "react";
import {EndTurnButton} from "./EndTurnButton";
import "./turn-timer-label.css"
export function TurnTimerLabel({playerName, time, disabled} : {playerName:string, time:string, disabled:boolean}){
    const turnEndTime = new Date(time);
    const timeDifference = turnEndTime.getTime() - Date.now();
    const secondsDifference = Math.floor(timeDifference / 1000);

    const minutesLeft = Math.floor(secondsDifference / 60);
    const secondsLeft = secondsDifference - minutesLeft * 60;

    const secondsLeftString =  secondsLeft > 9 ? secondsLeft.toString() : `0${secondsLeft}`;

    return (
        <div className="turn-label-div">
            <label> It's {playerName}'s turn </label>
            <label> {minutesLeft}:{secondsLeftString} </label>
            <EndTurnButton disabled={disabled}/>
        </div>
    )
}