import {GameSessionDto} from "../../../interfaces/GameSessionDto";
import {MapLogicInfo} from "../utils/MapLogicInfo";
import {usePlayer} from "../../../context/PlayerProvider";
// const getVisibleSettlementSpots = (): number[] => {
//     if (gameSession.round === 1 || gameSession.round === 2){
//         return getBeginningSettlementSpots();
//     }
//
//     const settlementPositions = gameSession.map.settlements.map(s => s.position);
//     const playerSettlementPosition = gameSession.map.settlements
//         .filter(s => s.playerId === player?.id)
//         .map(s => s.position)
//     const settlementSpotPositions : number[] = [];
//
//     for (const pos of playerSettlementPosition){
//         const settlements = adjacentSettlements[pos];
//         for (const settlement of settlements)
//             if (!settlementPositions.includes(settlement))
//                 settlementSpotPositions.push(settlement)
//     }
//
//     return settlementSpotPositions;
// };
const useVisibleSpots = (gameSession: GameSessionDto) => {


    const adjacentSettlements = MapLogicInfo.getAdjacentSettlements();
    const roadEnds = MapLogicInfo.getRoadEnds();
    const {player} = usePlayer();

    const getVisibleSettlementSpots = (): number[] => {
        if (gameSession.round === 1 || gameSession.round === 2){
            return getBeginningSettlementSpots();
        }

        const settlementPositions = gameSession.map.settlements.map(s => s.position);
        const playerRoadPosition = gameSession.map.roads
            .filter(s => s.playerId === player?.id)
            .map(s => s.position)
        const settlementSpotPositions : number[] = [];

        for (const road of playerRoadPosition) {
            const ends = roadEnds[road];

            for (const settlement of ends)
                if (!settlementPositions.includes(settlement)){
                    let ok = true
                    for (const adjacent of adjacentSettlements[settlement])
                        if (settlementPositions.includes(adjacent)){
                            ok = false;
                            break;
                        }
                    if(ok)
                        settlementSpotPositions.push(settlement)
                }
        }

        return settlementSpotPositions;
    };

    const getBeginningSettlementSpots = (): number[] => {
        const settlementPositionsSet = new Set<number>(Array.from({ length: 55 }, (_, index) => index));
        const settlementPositions = gameSession.map.settlements.map(s => s.position);
        for (const pos of settlementPositions) settlementPositionsSet.delete(pos);
        return Array.from(settlementPositionsSet);
    };

    return {
        getVisibleSettlementSpots
    };
};

export default useVisibleSpots;