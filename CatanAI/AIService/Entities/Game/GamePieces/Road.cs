using Catan.Data;

namespace AIService.Entities.Game.GamePieces
{
    public class Road : GamePiece
    {
        public static int GetRoadPosition(int settlementPosition1, int settlementPosition2)
        {
            if (settlementPosition1 < settlementPosition2)
                return GameMapData.RoadByRoadEnds[(settlementPosition1,  settlementPosition2)];
            else return GameMapData.RoadByRoadEnds[(settlementPosition2, settlementPosition1)];
        }
    }
}