import React from "react";
import {ActionButton} from "./ActionButton";

export function TradeButton ({disabled, who, onClick} : {disabled: boolean, who : string, onClick : () => void}) {
    return (
        <ActionButton
            disabled={disabled}
            text={'Trade '+who}
            onClick={onClick}
            className="road-button" />
    );
}
