import {PlaceSettlementButton} from "./PlaceSettlementButton";
import {PlaceRoadButton} from "./PlaceRoadButton";
import {TradeButton} from "./TradeButton";
import {PlaceCityButton} from "./PlaceCityButton";
import "./action-button.css"
import {BuyDevelopmentCardButton} from "./BuyDevelopmentCardButton";
import {Tooltip} from "../../misc/tooltip/ToolTip";
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
    handleBuyDevelopmentButtonClick: () => void;
}
interface ResourceRequirements {
    settlement: string;
    city: string;
    road: string;
    developmentCard: string;
}

const resourceRequirements: ResourceRequirements = {
    settlement: "2x Wood, 2x Brick",
    city: "3x Wheat, 2x Ore",
    road: "1x Wood, 1x Brick",
    developmentCard: "1x Wheat, 1x Sheep, 1x Ore"
};

function ActionBar({ disabled, activeButton, handlePlaceSettlementButtonClick, handlePlaceRoadButtonClick, handlePlaceCityButtonClick, handleTradeBankButtonClick, handleTradePlayerButtonClick, handleBuyDevelopmentButtonClick }: ActionBarProps) {
    return (
        <div className="actions-div">
            <div className="action-button-wrapper">
                <TradeButton disabled={disabled} onClick={handleTradeBankButtonClick} who='bank' />
            </div>
            <div className="action-button-wrapper">
                <TradeButton disabled={disabled} onClick={handleTradePlayerButtonClick} who='player' />
            </div>
            <div className="action-button-wrapper">
                <PlaceSettlementButton
                    disabled={disabled}
                    isActive={activeButton === ButtonActions.PlaceSettlement}
                    onClick={() => handlePlaceSettlementButtonClick(ButtonActions.PlaceSettlement)}
                />
                <Tooltip text={resourceRequirements.settlement} />
            </div>
            <div className="action-button-wrapper">
                <PlaceCityButton
                    disabled={disabled}
                    isActive={activeButton === ButtonActions.PlaceCity}
                    onClick={() => handlePlaceCityButtonClick(ButtonActions.PlaceCity)}
                />
                <Tooltip text={resourceRequirements.city} />
            </div>
            <div className="action-button-wrapper">
                <PlaceRoadButton
                    disabled={disabled}
                    isActive={activeButton === ButtonActions.PlaceRoad}
                    onClick={() => handlePlaceRoadButtonClick(ButtonActions.PlaceRoad)}
                />
                <Tooltip text={resourceRequirements.road} />
            </div>
            <div className="action-button-wrapper">
                <BuyDevelopmentCardButton
                    disabled={disabled}
                    isActive={false}
                    onClick={() => handleBuyDevelopmentButtonClick()}
                />
                <Tooltip text={resourceRequirements.developmentCard} />
            </div>
        </div>
    );
}

export {ActionBar};