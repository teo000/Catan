using Catan.Data;

namespace AIService.Entities.Game.GamePieces
{
    public class Road : GamePiece
    {
        public static int GetRoadPosition(int settlementPosition1, int settlementPosition2)
        {
            if (settlementPosition1 < settlementPosition2)
                return GameMapData.RoadByRoadEnds
                    .TryGetValue((settlementPosition1, settlementPosition2), out var roadPosition) 
                    ? roadPosition : -1;
            else
                return GameMapData.RoadByRoadEnds
                    .TryGetValue((settlementPosition2, settlementPosition1), out var roadPosition) 
                    ? roadPosition : -1;
		}
    }
}