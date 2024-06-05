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
    const roadByRoadEnds = MapLogicInfo.getRoadByRoadEnds();
    const {player} = usePlayer();

    const getVisibleSettlementSpots = (): number[] => {
        if (gameSession.round === 1 || gameSession.round === 2)
            return getBeginningSettlementSpots();


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

    const getVisibleRoadSpots = () : number[]=>{
        if (gameSession.round === 1 || gameSession.round === 2)
            return getBeginningRoadSpots();

        const visibleRoadSpots = [];

        const allRoads = gameSession.map.roads.map(r => r.position);

        const playerRoads = gameSession.map.roads
            .filter(r => r.playerId === player?.id)
            .map(r => r.position);

        for (const road of playerRoads){
            const ends = roadEnds[road];
            for (const end of ends) {
                for (const settlement of adjacentSettlements[end]) {
                    let key: [number, number] = [-1, -1];

                    if (end < settlement)
                        key = [end, settlement]
                    else key = [settlement, end]

                    const roadPosition = roadByRoadEnds.get(JSON.stringify(key));
                    if (roadPosition && !allRoads.includes(roadPosition))
                        visibleRoadSpots.push(roadPosition);
                }
            }
        }

        return visibleRoadSpots;

    };

    const getBeginningRoadSpots = () : number[] =>{
        const roadPositionsSet = new Set<number>(Array.from({ length: 73 }, (_, index) => index));
        const roadPositions = gameSession.map.roads.map(r => r.position);
        for (const pos of roadPositions) roadPositionsSet.delete(pos);
        return Array.from(roadPositionsSet);
    }

    return {
        getVisibleSettlementSpots,
        getVisibleRoadSpots
    };
};

export default useVisibleSpots;