using Catan.Data;

namespace AIService.Entities.Game.GamePieces
{
	public class Settlement : Building
	{
		public override int Points { get; set; } = 1;

		public static bool HasAdjacentSettlements(List<int> settlementsPositions, int newSettlementPosition)
		{
			foreach (var position in GameMapData.AdjacentSettlements[newSettlementPosition])
				if (settlementsPositions.Contains(position))
					return true;
			return false;
		}

	}
}