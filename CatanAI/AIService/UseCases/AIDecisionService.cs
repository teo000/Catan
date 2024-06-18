using AIService.Entities.Common;
using AIService.Entities.Game;
using AIService.Entities.Game.GameMap;
using AIService.Entities.Game.GamePieces;
using AIService.Entities.Moves;
using Catan.Data;

namespace AIService.UseCases
{
    public class AIDecisionService
	{
		private static Random rng = new Random();

		public Result<List<Move>> DecideNextMove(GameState gameState, Guid playerId)
		{
			var player = gameState.Players.Where(p => p.Id == playerId).FirstOrDefault();

			var moves = new List<Move>();
			if (gameState.Round == 1 || gameState.Round == 2)
				moves = HandleBeginning(gameState);
			else
				moves = HandleGame(gameState, player);
			return Result<List<Move>>.Success(moves);
		}

		private List<Move> HandleGame(GameState gameState, Player player)
		{
			var moves = new List<Move>();
			var map = gameState.Map;


			if (player.HasResources(Buyable.CITY)) {
				var settlementToUpgrade = ListExtensions.GetRandomElement(player.Settlements);
				moves.Add(new PlaceCityMove(gameState.Id, settlementToUpgrade.Position));
				player.SubtractResources(Buyable.CITY);
			}
			
			if (player.HasResources(Buyable.SETTLEMENT))
			{
				var viableSettlementPositions = Map.GetViableSettlementPositions(gameState, player);
				var newSettlementPosition = ListExtensions.GetRandomElement(viableSettlementPositions);
				moves.Add(new PlaceSettlementMove(gameState.Id, newSettlementPosition));
				player.SubtractResources(Buyable.ROAD);
			}

			if (player.HasResources(Buyable.ROAD))
			{
				var viableRoadPositions = Map.GetViableRoadPositions(gameState, player);
				var newRoadPosition = ListExtensions.GetRandomElement(viableRoadPositions);
				moves.Add(new PlaceRoadMove(gameState.Id, newRoadPosition));
				player.SubtractResources(Buyable.ROAD);
			}


			return moves;
		}

		private List<Move> HandleBeginning(GameState gameState)
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

			return moves;
		}

	}
}
