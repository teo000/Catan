import React from "react";
import {ActionButton, CustomButtonProps} from "./ActionButton";

export const BuyDevelopmentCardButton: React.FC<CustomButtonProps> = ({ disabled, isActive, onClick }) => {

    const handleClick = () => {
        console.log('Buy development card clicked');
        onClick();
    };

    return <ActionButton text="Buy Development Card"
                         disabled={disabled}
                         isActive={isActive}
                         onClick={handleClick}
                         className="development-button" />;
};