import React from "react";
import {ActionButton} from "./ActionButton";
interface CustomButtonProps {
    isActive: boolean;
    onClick: () => void;
}

export const PlaceSettlementButton: React.FC<CustomButtonProps> = ({ isActive, onClick }) => {

    const handleClick = () => {
        console.log('Place Settlement button clicked');
        onClick();
    };

    return <ActionButton text="Place settlement" isActive={isActive} onClick={handleClick} className="settlement-button" />;
};
