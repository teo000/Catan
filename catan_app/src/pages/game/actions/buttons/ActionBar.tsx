import {PlaceSettlementButton} from "./PlaceSettlementButton";
import {PlaceRoadButton} from "./PlaceRoadButton";
import {TradeButton} from "./TradeButton";
import {PlaceCityButton} from "./PlaceCityButton";
import "./action-button.css"
export enum ButtonActions {
    None = 'None',
    PlaceSettlement = 'PlaceSettlement',
    PlaceRoad = 'PlaceRoad',
    PlaceCity = 'PlaceCity',
    Trade = 'Trade',
}

interface ActionBarProps {
    disabled: boolean
    activeButton: ButtonActions;
    handlePlaceSettlementButtonClick: (action: ButtonActions) => void;
    handlePlaceRoadButtonClick: (action: ButtonActions) => void;
    handlePlaceCityButtonClick: (action: ButtonActions) => void;
    handleTradeBankButtonClick: () => void;
    handleTradePlayerButtonClick : () => void;
}

function ActionBar({ disabled,
                       activeButton,
                       handlePlaceSettlementButtonClick ,
                       handlePlaceRoadButtonClick,
                       handlePlaceCityButtonClick,
                       handleTradeBankButtonClick,
                       handleTradePlayerButtonClick
}: ActionBarProps) {
    return (
        <div className="actions-div">
            <TradeButton disabled={disabled} onClick = {handleTradeBankButtonClick} who='bank'/>
            <TradeButton disabled={disabled} onClick = {handleTradePlayerButtonClick} who='player'/>
            <PlaceSettlementButton
                disabled={disabled}
                isActive={activeButton === ButtonActions.PlaceSettlement}
                onClick={() => handlePlaceSettlementButtonClick(ButtonActions.PlaceSettlement)}
            />
            <PlaceCityButton
                disabled={disabled}
                isActive={activeButton === ButtonActions.PlaceCity}
                onClick={() => handlePlaceCityButtonClick(ButtonActions.PlaceCity)}
            />
            <PlaceRoadButton
                disabled={disabled}
                isActive={activeButton === ButtonActions.PlaceRoad}
                onClick={() => handlePlaceRoadButtonClick(ButtonActions.PlaceRoad)}
            />
        </div>
    );
}

export {ActionBar};