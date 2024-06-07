import React from "react";
import {ActionButton, CustomButtonProps} from "./ActionButton";



export const PlaceRoadButton: React.FC<CustomButtonProps> = ({ disabled, isActive, onClick }) => {

    const handleClick = () => {
        console.log('Place Road button clicked');
        onClick();
    };

    return (
        <ActionButton
            text="Place road"
            disabled={disabled}
            isActive={isActive}
            onClick={handleClick}
            className="road-button" />
    );
};
