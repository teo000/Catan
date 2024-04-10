import React, { useState } from 'react';

const GamePiecePaths : Record<string, string>={
    CITY: 'images/cities/red.png',
    SETTLEMENT: 'images/settlements/red.png',
    ROAD: 'images/roads/red.png',
}
function ActionButton({pieceType} : {pieceType: string}) {
    const [clicked, setClicked] = useState(false);

    const handleClick = () => {
        setClicked(!clicked);
    };

    return (
        <button className={`action-button ${clicked ? 'clicked' : ''}`} onClick={handleClick}>
            <img src={GamePiecePaths[pieceType.toUpperCase()]} alt="Icon" className="button-icon"/>
        </button>
    );
}
export {ActionButton};