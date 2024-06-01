import {ActionButton} from "./ActionButton";
import {useState} from "react";
import {PlaceSettlementButton} from "./PlaceSettlementButton";
import {PlaceRoadButton} from "./PlaceRoadButton";
import {TradeButton} from "./TradeButton";
import {PlaceCityButton} from "./PlaceCityButton";

export enum ButtonActions {
    None = 'None',
    PlaceSettlement = 'PlaceSettlement',
    PlaceRoad = 'PlaceRoad',
    PlaceCity = 'PlaceCity',
    Trade = 'Trade',
}

interface ActionBarProps {
    activeButton: ButtonActions;
    handlePlaceSettlementButtonClick: (action: ButtonActions) => void;
    handlePlaceRoadButtonClick: (action: ButtonActions) => void;
    handlePlaceCityButtonClick: (action: ButtonActions) => void;
    handleTradeBankButtonClick: () => void;
    handleTradePlayerButtonClick : () => void;
}

function ActionBar({ activeButton,
                       handlePlaceSettlementButtonClick ,
                       handlePlaceRoadButtonClick,
                       handlePlaceCityButtonClick,
                       handleTradeBankButtonClick,
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
            <PlaceCityButton
                isActive={activeButton === ButtonActions.PlaceCity}
                onClick={() => handlePlaceCityButtonClick(ButtonActions.PlaceCity)}
            />
            <PlaceRoadButton
                isActive={activeButton === ButtonActions.PlaceRoad}
                onClick={() => handlePlaceRoadButtonClick(ButtonActions.PlaceRoad)}
            />
        </div>
    );
}

export {ActionBar};