import React from "react";
import {ActionButton, CustomButtonProps} from "./ActionButton";

export const PlaceCityButton: React.FC<CustomButtonProps> = ({ disabled, isActive, onClick }) => {

    const handleClick = () => {
        console.log('Place City button clicked');
        onClick();
    };

    return <ActionButton text="Place city" disabled={disabled} isActive={isActive} onClick={handleClick} className="city-button" />;
};
