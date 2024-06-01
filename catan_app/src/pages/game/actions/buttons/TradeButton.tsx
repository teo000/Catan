import React, {useState} from "react";
import {ActionButton} from "./ActionButton";

export function TradeButton ({who, onClick} : {who : string, onClick : () => void}) {
    return <ActionButton text={'Trade '+who} onClick={onClick} className="road-button" />;
}
