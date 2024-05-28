import React, {useState} from "react";
import {ActionButton} from "./ActionButton";

export function TradeButton ({onClick} : {onClick : () => void}) {


    return <ActionButton text="Trade bank" onClick={onClick} className="road-button" />;
}
