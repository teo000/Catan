import {useState} from "react";
import {ButtonActions} from "../action-button/ActionBar";

interface SettlementSpotProps {
    left: number,
    top: number,
    index: number,
    isSettlement:boolean,
    onClick: (id: number) => void
}

function SettlementSpot({left, top, index, isSettlement, onClick} : SettlementSpotProps){

    return (
        <div className={isSettlement? 'settlement' : 'settlement-spot'}
             onClick = {() => onClick(index)}
                style={{
                    left: `${left}px`,
                    top: `${top}px`
                }}>
        </div>
    )
}

export {SettlementSpot};