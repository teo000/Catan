import React, { useState } from 'react';

const GamePiecePaths : Record<string, string>={
    CITY: 'images/cities/red.png',
    SETTLEMENT: 'images/settlements/red.png',
    ROAD: 'images/roads/red.png',
}

interface ActionButtonProps {
    disabled: boolean;
    text: string;
    isActive?: boolean;
    onClick: () => void;
    className: string;
}

export interface CustomButtonProps {
    disabled: boolean;
    isActive: boolean;
    onClick: () => void;
}

const ActionButton: React.FC<ActionButtonProps> =
    ({disabled,  text, isActive, onClick, className }) => {
    return (
        <button
            className={` ${disabled? 'disabled' : (isActive ? 'clicked' : '')} action-button ${className} `}
            onClick={onClick}
        >
            {text}
        </button>
    );
};

export {ActionButton};