import {GameSessionDto} from "../../../interfaces/GameSessionDto";

const useDetermineVisibleSpots = (gameSession: GameSessionDto) => {
    const determineVisibleSettlementSpots = (): number[] => {
        const settlementPositionsSet = new Set<number>(Array.from({ length: 55 }, (_, index) => index));
        const settlementPositions = gameSession.map.settlements.map(s => s.position);
        for (const pos of settlementPositions) settlementPositionsSet.delete(pos);
        return Array.from(settlementPositionsSet);
    };

    return {
        determineVisibleSettlementSpots
    };
};

export default useDetermineVisibleSpots;