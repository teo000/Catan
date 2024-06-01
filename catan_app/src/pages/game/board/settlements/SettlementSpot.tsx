import {useState} from "react";
import {ButtonActions} from "../../actions/buttons/ActionBar";

interface SettlementSpotProps {
    left: number,
    top: number,
    index: number,
    onClick: (id: number) => void
}

function SettlementSpot({left, top, index, onClick} : SettlementSpotProps){

    return (
        <div className='settlement-spot'
             onClick = {() => onClick(index)}
                style={{
                    left: `${left}px`,
                    top: `${top}px`
                }}>
        </div>
    )
}

export {SettlementSpot};