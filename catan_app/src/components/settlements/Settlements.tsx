import {SettlementSpotInfo} from "./ComputeSettlementSpotsInfo";
import {Settlement} from "./Settlement";
import {SettlementDto} from "../../interfaces/SettlementDto";
import {getPlayerColor, Player, PlayerColor} from "../../interfaces/Player";

interface SettlementsProps{
    settlementSpotInfo : SettlementSpotInfo[],
    settlements : SettlementDto[],
    players: Player[]
}

export function Settlements({settlementSpotInfo, settlements, players} : SettlementsProps){
    const settlementIds = settlements.map(settlement=> settlement.position);

    const playerColorDict: { [id: number]: PlayerColor } = settlements.reduce((dict, settlement) => {
        let player = players.find(player => player.id === settlement.playerId);
        if (!player)
            throw new Error("All settlements could not be loaded.");
        dict[settlement.position] = getPlayerColor(player.color);
        return dict;
    }, {} as { [id: number]: PlayerColor });


    return (
        <div className="settlements">
            {settlementSpotInfo.map(settlement => (
                settlementIds.includes(settlement.id) && (
                    <Settlement key={settlement.id}
                                    left={settlement.left}
                                    top={settlement.top}
                                    index={settlement.id}
                                    color={playerColorDict[settlement.id]}
                    />
                )
            ))}
        </div>
    );
}