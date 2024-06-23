import {useState} from "react";

interface RoadSpotProps {
    index: number,
    top: number,
    left: number,
    rotation: number,
    onClick: (id: number) => void
}

function RoadSpot({index, top, left, rotation, onClick} : RoadSpotProps){

    return (
        <div
            className="road-spot"
            onClick={() => (onClick(index))}
            style={{
                transform: `rotate(${rotation}deg)`,
                top: `${top}px` ,
                left: `${left}px`
            }}
        >

        </div>
    );
}

export {RoadSpot};