import {ComputeSettlementSpotRow} from "./ComputeSettlementSpotRowInfo";
import {SettlementSpot} from "./SettlementSpot";

interface SettlementSpotsProps {
    visibleSettlements: number[];
    isSettlement: boolean[];
    onSettlementClick: (id: number) => void;
}

function SettlementSpots({ visibleSettlements, isSettlement, onSettlementClick }: SettlementSpotsProps) {

    let settlements = [];
    let startingNumber = 0;

    for (let i = 1; i <= 12; i++) {
        let row = ComputeSettlementSpotRow(i, startingNumber);
        startingNumber += row.length;
        settlements.push(...row);
    }

    console.log(settlements)

    return (
        <div className="settlement-spots">
            {settlements.map(settlement => (
                visibleSettlements.includes(settlement.id) && (
                    <SettlementSpot key={settlement.id}
                                   left={settlement.left}
                                   top={settlement.top}
                                   index={settlement.id}
                                   isSettlement={isSettlement[settlement.id]}
                                   onClick={() => onSettlementClick(settlement.id)}
                    />
                )
            ))}
        </div>
    );
}

export {SettlementSpots}