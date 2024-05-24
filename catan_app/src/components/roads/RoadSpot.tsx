import {useState} from "react";

interface RoadSpotProps {
    index: number,
    top: number,
    left: number,
    rotation: number,
    isRoad: boolean,
    onClick: (id: number) => void
}

function RoadSpot({index, top, left, rotation, isRoad, onClick} : RoadSpotProps){

    return (
        <div
            className={isRoad ? "road" : "road-spot" }
            onClick={() => (onClick(index))}
            style={{
                transform: `rotate(${rotation}deg)`,
                top: `${ isRoad?  `${top - 12}px` : `${top}px`  }`,
                left: `${left}px`
            }}
        />
    );
}

export {RoadSpot};