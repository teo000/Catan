import {ComputeSettlementSpotRow, SettlementSpotInfo} from "./ComputeSettlementSpotsInfo";
import {SettlementSpot} from "./SettlementSpot";

interface SettlementSpotsProps {
    settlementSpotInfo: SettlementSpotInfo[];
    visibleSettlementSpots: number[];
    onSettlementClick: (id: number) => void;
}


function SettlementSpots({ settlementSpotInfo, visibleSettlementSpots , onSettlementClick }: SettlementSpotsProps) {

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