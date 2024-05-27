import React from "react";

interface DotPosition {
    x: number;
    y: number;
}

interface DotPositionMatrix {
    [key: number]: DotPosition[];
}


function Dice({number}: {number: number | null}){
    const dotPositionMatrix : DotPositionMatrix = {
        0: [],
        1: [{ x: 50, y: 50 }],
        2: [{ x: 20, y: 20 }, { x: 80, y: 80 }],
        3: [{ x: 20, y: 20 }, { x: 50, y: 50 }, { x: 80, y: 80 }],
        4: [{ x: 20, y: 20 }, { x: 80, y: 20 }, { x: 20, y: 80 }, { x: 80, y: 80 }],
        5: [{ x: 20, y: 20 }, { x: 80, y: 20 }, { x: 50, y: 50 }, { x: 20, y: 80 }, { x: 80, y: 80 }],
        6: [{ x: 20, y: 20 }, { x: 80, y: 20 }, { x: 20, y: 50 }, { x: 80, y: 50 }, { x: 20, y: 80 }, { x: 80, y: 80 }]
    };

    if (number === null)
        return <div className="dice"></div>

    return (
        <div className="dice">
            {
                dotPositionMatrix[number].map((dotPosition, index) => (
                <div
                    key={index}
                    className="dice-dot"
                    style={{ '--top': `${dotPosition.y}%`, '--left': `${dotPosition.x}%` } as React.CSSProperties}
                />
            ))}
        </div>
    )
}

export {Dice};

// (<div className="dice-dot"></div>)
