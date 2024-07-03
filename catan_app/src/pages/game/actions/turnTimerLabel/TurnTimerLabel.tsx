import React from "react";
import {EndTurnButton} from "./EndTurnButton";
import "./turn-timer-label.css"
import {Timer} from "./Timer";
export function TurnTimerLabel({playerName, time, disabled} : {playerName:string, time:string, disabled:boolean}){


    return (
        <div className="turn-label-div">
            <label> It's {playerName}'s turn </label>
            <Timer time={time}/>
            <EndTurnButton disabled={disabled}/>
        </div>
    )
}