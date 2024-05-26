import {ActionButton} from "./ActionButton";
import {useState} from "react";
import {PlaceSettlementButton} from "./PlaceSettlementButton";
import {PlaceRoadButton} from "./PlaceRoadButton";

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
}

function ActionBar({ activeButton, handlePlaceSettlementButtonClick , handlePlaceRoadButtonClick}: ActionBarProps) {
    return (
        <div className="actions-div">
            {/*<TradeButton isActive={activeButton === ButtonActions.Trade} onClick={() => handleButtonClick(ButtonActions.Trade)} />*/}
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