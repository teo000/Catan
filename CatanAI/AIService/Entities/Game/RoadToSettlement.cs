namespace AIService.Entities.Game
{
	public class RoadToSettlement
	{
		public int SettlementPosition { get; set; }
		public List<int> Roads { get; set; }

		public RoadToSettlement(int settlementPosition, List<int> roads)
		{
			SettlementPosition = settlementPosition;
			Roads = roads;
		}
	}
}
