import {SettlementSpotInfo} from "./ComputeSettlementSpotsInfo";
import {Settlement} from "./Settlement";
import {SettlementDto} from "../../interfaces/SettlementDto";

export function Settlements({settlementSpotInfo, settlements} : {settlementSpotInfo : SettlementSpotInfo[], settlements : SettlementDto[]}){
    const settlementIds = settlements.map(settlement=> settlement.position);

    return (
        <div className="settlement-spots">
            {settlementSpotInfo.map(settlement => (
                settlementIds.includes(settlement.id) && (
                    <Settlement key={settlement.id}
                                    left={settlement.left}
                                    top={settlement.top}
                                    index={settlement.id}
                    />
                )
            ))}
        </div>
    );
}