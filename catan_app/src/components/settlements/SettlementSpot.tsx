import {useState} from "react";

function SettlementSpot({left, top} : {left: number, top: number}){
    const [isSettlement, setIsSettlement] = useState(false);
    const handleClick = () => {
        setIsSettlement(true);
    };

    return (
        <div className={isSettlement? 'settlement' : 'settlement-spot'}
             onClick = {handleClick}
                style={{
                    left: `${left}px`,
                    top: `${top}px`
                }}>
        </div>
    )
}

export {SettlementSpot};