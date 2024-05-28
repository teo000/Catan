import {ActionButton} from "./ActionButton";
import {useState} from "react";
import {PlaceSettlementButton} from "./PlaceSettlementButton";
import {PlaceRoadButton} from "./PlaceRoadButton";
import {TradeButton} from "./TradeButton";

export enum ButtonActions {
    None = 'None',
    PlaceSettlement = 'PlaceSettlement',
    PlaceRoad = 'PlaceRoad',
    Trade = 'Trade',
}

interface ActionBarProps {
    activeButton: ButtonActions;
    handlePlaceSettlementButtonClick: (action: ButtonActions) => void;
    handlePlaceRoadButtonClick: (action: ButtonActions) => void;
    handleTradeBankButtonClick: () => void;
    handleTradePlayerButtonClick : () => void;
}

function ActionBar({ activeButton,
                       handlePlaceSettlementButtonClick ,
                       handlePlaceRoadButtonClick, handleTradeBankButtonClick,
                       handleTradePlayerButtonClick
}: ActionBarProps) {
    return (
        <div className="actions-div">
            <TradeButton onClick = {handleTradeBankButtonClick} who='bank'/>
            <TradeButton onClick = {handleTradePlayerButtonClick} who='player'/>
            <PlaceSettlementButton
                isActive={activeButton === ButtonActions.PlaceSettlement}
                onClick={() => handlePlaceSettlementButtonClick(ButtonActions.PlaceSettlement)}
            />
            <PlaceRoadButton
                isActive={activeButton === ButtonActions.PlaceRoad}
                onClick={() => handlePlaceRoadButtonClick(ButtonActions.PlaceRoad)}
            />
        </div>
    );
}

export {ActionBar};