import {SettlementSpotInfo} from "./ComputeSettlementSpotsInfo";
import {Settlement} from "./Settlement";
import {SettlementDto} from "../../../../interfaces/SettlementDto";
import {getPlayerColor, PlayerDto, PlayerColor} from "../../../../interfaces/PlayerDto";

import "./settlements.css";
import {MapDrawInfo} from "../../utils/MapDrawInfo";

interface SettlementsProps{
    settlements : SettlementDto[],
    players: PlayerDto[]
}

export function Settlements({settlements, players} : SettlementsProps){
    const settlementIds = settlements.map(settlement=> settlement.position);

    const playerColorDict: { [id: number]: PlayerColor } = settlements.reduce((dict, settlement) => {
        let player = players.find(player => player.id === settlement.playerId);
        if (!player)
            throw new Error("All settlements could not be loaded.");
        dict[settlement.position] = getPlayerColor(player.color);
        return dict;
    }, {} as { [id: number]: PlayerColor });

    const settlementSpotInfo = MapDrawInfo.SettlementSpotInfo;

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