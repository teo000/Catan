import {SettlementSpotInfo} from "./ComputeSettlementSpotsInfo";
import {SettlementSpot} from "./SettlementSpot";
import {MapInfo} from "../../utils/MapInfo";

interface SettlementSpotsProps {
    visibleSettlementSpots: number[];
    onSettlementClick: (id: number) => void;
}


function SettlementSpots({visibleSettlementSpots , onSettlementClick }: SettlementSpotsProps) {
    const settlementSpotInfo = MapInfo.SettlementSpotInfo;

    return (
        <div className="settlement-spots">
            {settlementSpotInfo.map(settlement => (
                visibleSettlementSpots.includes(settlement.id) && (
                    <SettlementSpot key={settlement.id}
                                   left={settlement.left}
                                   top={settlement.top}
                                   index={settlement.id}
                                   onClick={() => onSettlementClick(settlement.id)}
                    />
                )
            ))}
        </div>
    );
}

export {SettlementSpots}