import React from "react";
import {ActionButton, CustomButtonProps} from "./ActionButton";
export const PlaceSettlementButton: React.FC<CustomButtonProps> = ({ disabled, isActive, onClick }) => {

    const handleClick = () => {
        console.log('Place Settlement button clicked');
        onClick();
    };

    return (
        <ActionButton
            text="Place settlement"
            disabled={disabled}
            isActive={isActive}
            onClick={handleClick}
            className="settlement-button"
        />);
};
