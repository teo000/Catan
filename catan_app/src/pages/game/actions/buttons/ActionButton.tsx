import React, { useState } from 'react';

const GamePiecePaths : Record<string, string>={
    CITY: 'images/cities/red.png',
    SETTLEMENT: 'images/settlements/red.png',
    ROAD: 'images/roads/red.png',
}

interface ActionButtonProps {
    text: string;
    isActive?: boolean;
    onClick: () => void;
    className: string;
}

export interface CustomButtonProps {
    isActive: boolean;
    onClick: () => void;
}

const ActionButton: React.FC<ActionButtonProps> =
    ({ text, isActive, onClick, className }) => {
    return (
        <button
            className={`action-button ${className} ${isActive ? 'clicked' : ''}`}
            onClick={onClick}
        >
            {text}
        </button>
    );
};

export {ActionButton};