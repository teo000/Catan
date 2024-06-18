using AIService.Entities.Common;
using AIService.Entities.Game;
using AIService.Entities.Game.GamePieces;
using AIService.Entities.Moves;
using Catan.Data;

namespace AIService.UseCases
{
    public class AIDecisionService
	{
		private static Random rng = new Random();

		public Result<List<Move>> DecideNextMove(GameState gameState)
		{
			if (gameState.Round == 1 || gameState.Round == 2)
				return HandleBeginning(gameState);


			return Result<List<Move>>.Success([new PlaceSettlementMove(gameState.Id, 0)]);
		}

		private Result<List<Move>> HandleBeginning(GameState gameState)
		{
			var moves = new List<Move>();

			var map = gameState.Map;

			var settlementsPositions = map.Settlements.Select(s => s.Position).ToList();

			var newSettlementPosition = -1;
			do
			{
				newSettlementPosition = rng.Next(GameMapData.SETTLEMENTS_NO - 1);
			} while (Settlement.HasAdjacentSettlements(settlementsPositions, newSettlementPosition));

			moves.Add(new PlaceSettlementMove(gameState.Id, newSettlementPosition));

			var adjacentSettlements = GameMapData.AdjacentSettlements[newSettlementPosition];
			var otherSettlementPosition = ListExtensions.GetRandomElement(adjacentSettlements);
			var roadPosition = Road.GetRoadPosition(newSettlementPosition, otherSettlementPosition);

			moves.Add(new PlaceRoadMove(gameState.Id, roadPosition));

			return Result<List<Move>>.Success(moves);
		}

	}
}
