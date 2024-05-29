import React from "react";
import {ActionButton, CustomButtonProps} from "./ActionButton";



export const PlaceRoadButton: React.FC<CustomButtonProps> = ({ isActive, onClick }) => {

    const handleClick = () => {
        console.log('Place Road button clicked');
        onClick();
    };

    return <ActionButton text="Place road" isActive={isActive} onClick={handleClick} className="road-button" />;
};
