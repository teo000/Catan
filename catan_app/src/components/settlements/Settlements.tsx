import {SettlementSpotInfo} from "./ComputeSettlementSpotsInfo";
import {Settlement} from "./Settlement";

export function Settlements({settlementSpotInfo, settlementIds} : {settlementSpotInfo : SettlementSpotInfo[], settlementIds : number[]}){
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