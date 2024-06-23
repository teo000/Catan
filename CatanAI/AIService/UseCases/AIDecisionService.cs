using AIService.Entities.Common;
using AIService.Entities.Data;
using AIService.Entities.Game;
using AIService.Entities.Game.GameMap;
using AIService.Entities.Game.GamePieces;
using AIService.Entities.Game.Trades;
using AIService.Entities.Moves;
using AIService.Utils;
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

		public Result<Dictionary<Resource, int>> DiscardHalf(GameState gameState, Guid playerId) 
		{
			var player = gameState.Players.Where(p => p.Id == playerId).FirstOrDefault();

			var resourceCount = player.ResourceCount;

			int initialSum = resourceCount.Sum(kv => kv.Value);
			int targetSum = initialSum / 2;

			var sortedResources = resourceCount.OrderByDescending(kv => kv.Value).ToList();

			var resourcesToDiscard = GameUtils.GetEmptyResourceDictionary();

			int currentSum = initialSum;
			foreach (var (resource, count) in sortedResources)
			{
				if (currentSum <= targetSum)
					break;

				int countToDiscard = Math.Min(currentSum - targetSum, resourceCount[resource]);
				resourcesToDiscard[resource] = countToDiscard;
				currentSum -= countToDiscard;
			}
			 
			return Result<Dictionary<Resource, int>>.Success(resourcesToDiscard);

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
				if (viableSettlementPositions.Count > 0)
				{
					var newSettlementPosition = ListExtensions.GetRandomElement(viableSettlementPositions);
					moves.Add(new PlaceSettlementMove(gameState.Id, newSettlementPosition));
					player.SubtractResources(Buyable.ROAD);
				}
			}

			if (player.HasResources(Buyable.ROAD))
			{
				var viableRoadPositions = Map.GetViableRoadPositions(gameState, player);
				if (viableRoadPositions.Count > 0)
				{
					var newRoadPosition = ListExtensions.GetRandomElement(viableRoadPositions);
					moves.Add(new PlaceRoadMove(gameState.Id, newRoadPosition));
					player.SubtractResources(Buyable.ROAD);
				}
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
			} while (!settlementsPositions.Contains(newSettlementPosition) 
					&& Settlement.HasAdjacentSettlements(settlementsPositions, newSettlementPosition));

			moves.Add(new PlaceSettlementMove(gameState.Id, newSettlementPosition));

			var adjacentSettlements = GameMapData.AdjacentSettlements[newSettlementPosition];
			var otherSettlementPosition = ListExtensions.GetRandomElement(adjacentSettlements);
			var roadPosition = Road.GetRoadPosition(newSettlementPosition, otherSettlementPosition);

			moves.Add(new PlaceRoadMove(gameState.Id, roadPosition));

			return moves;
		}

		public Result<bool> RespondToTrade (GameState gameState, Guid playerId, PlayerTrade trade)
		{
			return Result<bool>.Success(true);
		}
	}
}
