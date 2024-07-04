using AIService.Entities.Common;
using AIService.Entities.Data;
using AIService.Entities.Game;
using AIService.Entities.Game.GameMap;
using AIService.Entities.Game.GamePieces;
using AIService.Entities.Game.Trades;
using AIService.Entities.Moves;
using AIService.UseCases.Dtos;
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
				moves = HandleBeginning(gameState, player);
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

		
		public Result<bool> RespondToTrade (GameState gameState, Guid playerId, PlayerTrade trade)
		{
			var aIPlayer = gameState.Players.Where(p => p.Id == playerId).FirstOrDefault();

			var neededForCity = aIPlayer.CardsNeeded(Buyable.CITY);
			var neededForSettlement = aIPlayer.CardsNeeded(Buyable.SETTLEMENT);
			var neededForRoad = aIPlayer.CardsNeeded(Buyable.ROAD);

			if (!neededForCity.ContainsKey(trade.ResourceToReceive)
				&& !neededForSettlement.ContainsKey(trade.ResourceToReceive)
				&& !neededForCity.ContainsKey(trade.ResourceToReceive))
				return Result<bool>.Success(false);

			return Result<bool>.Success(true);

		}

		public Result<int> MoveThief(GameState gameState, Guid playerId)
		{
			var hexes = gameState.Map.HexTiles;
			var thiefPosition = gameState.Map.ThiefPosition;
			var otherPlayersSettlementPos = gameState.Map.Settlements
				.Where(p => p.PlayerId != playerId)
				.Select(s => s.Position)
				.ToList();

			var possibleHexes = otherPlayersSettlementPos
				.SelectMany(pos => GameMapData.SettlementAdjacentHexes[pos])
				.ToList();

			possibleHexes.Sort();


			var probabilityDict = new Dictionary<int, int>();

			foreach (int hex in possibleHexes)
			{
				if (!probabilityDict.ContainsKey(hex))
					probabilityDict.Add(hex, hexes[hex].Number);
				else probabilityDict[hex] += hexes[hex].Number;
			}

			var selectedHex = RandomExtensions.SelectWeightedRandom(probabilityDict);

			return Result<int>.Success(selectedHex);
		}

		public Result<List<PlayerTradeRequest>> InitiatePlayerTrades(GameState gameState, Guid playerId)
		{
			var aIPlayer = gameState.Players.Where(p => p.Id == playerId).FirstOrDefault();

			if (aIPlayer == null)
				return Result<List<PlayerTradeRequest>>.Failure("Player not found");

			if (aIPlayer.GetCardsNo() < 4 )
				return Result<List<PlayerTradeRequest>>.Failure("Player has less than 4 cards");

			if (aIPlayer.HasResources(Buyable.CITY)
				|| aIPlayer.HasResources(Buyable.CITY) || aIPlayer.HasResources(Buyable.ROAD))
				return Result<List<PlayerTradeRequest>>.Failure("Player has enough resources");

			var neededForCity = aIPlayer.CardsNeeded(Buyable.CITY);
			var neededForSettlement = aIPlayer.CardsNeeded(Buyable.SETTLEMENT);
			var neededForRoad = aIPlayer.CardsNeeded(Buyable.ROAD);

			var closestBuyable = new Dictionary<Buyable, Dictionary<Resource, int>>
			{
				{ Buyable.CITY, neededForCity },
				{ Buyable.SETTLEMENT, neededForSettlement },
				{ Buyable.ROAD, neededForRoad }
			}
			.Where(kv => kv.Value.Any())
			.OrderBy(kv => kv.Value.Values.Sum())
			.FirstOrDefault();

			if (closestBuyable.Key == default)
				return Result<List<PlayerTradeRequest>>.Failure("Player is not close enough to any buyable item");

			var trades = new List<PlayerTradeRequest>();

			foreach (var neededResource in closestBuyable.Value)
			{
				var resourceWanted = neededResource.Key;
				var quantityWanted = neededResource.Value;

				for (int i = 0; i < quantityWanted; i++)
				{
					var resourceOffered = aIPlayer.GetMostAbundantResource();
					if (!resourceOffered.HasValue) 
						continue;

					foreach (var p in gameState.Players) 
						if (p.Id != aIPlayer.Id)
						{
							var trade = new PlayerTradeRequest
							{
								PlayerToReceiveId = p.Id,
								ResourceToReceive = resourceWanted.ToString(),
								CountToReceive = quantityWanted,
								PlayerToGiveId = aIPlayer.Id,
								ResourceToGive = resourceOffered.Value.ToString(),
								CountToGive = quantityWanted
							};

							trades.Add(trade);
						}
				}
			}

			return Result<List<PlayerTradeRequest>>.Success(trades);

		}

		
		private List<Move> HandleGame(GameState gameState, Player player)
		{
			var moves = new List<Move>();

			if (player.HasResources(Buyable.CITY))
			{
				var settlementToUpgrade = player.Settlements
					.OrderByDescending(s => gameState.Map.GetSettlementResourceValue(s.Position))
					.FirstOrDefault();

				if (settlementToUpgrade != null)
				{
					moves.Add(new PlaceCityMove(gameState.Id, settlementToUpgrade.Position));
					player.SubtractResources(Buyable.CITY);
				}
			}

			if (player.HasResources(Buyable.SETTLEMENT))
			{
				var newSettlementPosition = GetBestSettlementPos(gameState, player);

				if (newSettlementPosition != -1)
				{
					moves.Add(new PlaceSettlementMove(gameState.Id, newSettlementPosition));
					player.SubtractResources(Buyable.SETTLEMENT);
				}

			}

			if (player.HasResources(Buyable.ROAD) && (GetBestSettlementPos(gameState, player) == -1 || rng.NextDouble() > 0.5))
			{
				var newRoadPosition = GetBestRoadPos(gameState, player);
				if (newRoadPosition != -1)
				{
					moves.Add(new PlaceRoadMove(gameState.Id, newRoadPosition));
					player.SubtractResources(Buyable.ROAD);
				}
			}


			return moves;
		}

		private List<Move> HandleBeginning(GameState gameState, Player player)
		{
			var moves = new List<Move>();

			var map = gameState.Map;

			var settlementsPositions = map.Settlements.Select(s => s.Position).ToList();

			var newSettlementPosition = GetBestBeginningSettlementPos(gameState, player);

			moves.Add(new PlaceSettlementMove(gameState.Id, newSettlementPosition));

			var adjacentSettlements = GameMapData.AdjacentSettlements[newSettlementPosition];
			var otherSettlementPosition = RandomExtensions.GetRandomElement(adjacentSettlements);
			var roadPosition = Road.GetRoadPosition(newSettlementPosition, otherSettlementPosition);

			moves.Add(new PlaceRoadMove(gameState.Id, roadPosition));

			return moves;
		}

		private int GetBestSettlementPos(GameState gameState, Player player)
		{
			var viableSettlementPositions = gameState.Map.GetViableSettlementPositions(player);

			if (viableSettlementPositions.Count == 0)
				return -1;

			var newSettlementPosition = viableSettlementPositions
				.OrderByDescending(gameState.Map.GetSettlementResourceValue)
				.FirstOrDefault();

			if (newSettlementPosition == null)
				return -1;

			return newSettlementPosition;
		}

		private int GetBestBeginningSettlementPos(GameState gameState, Player player)
		{
			var viableSettlementPositions = gameState.Map.GetViableBeginningSettlementPositions();

			if (viableSettlementPositions.Count == 0)
				return -1;

			var newSettlementPosition = viableSettlementPositions
				.OrderByDescending(gameState.Map.GetSettlementResourceValue)
				.FirstOrDefault();

			if (newSettlementPosition == null)
				return -1;

			return newSettlementPosition;
		}

		private int GetBestRoadPos(GameState gameState, Player player)
		{
			var roadsToSettlement = gameState.Map.GetViableRoadsToSettlement(player);

			if (roadsToSettlement.Count == 0)
				return -1;

			var bestRoadPositions = roadsToSettlement
				.OrderByDescending(rs => gameState.Map.GetSettlementResourceValue(rs.SettlementPosition))
				.Select(rs => rs.Roads)
				.ToList();

			if (bestRoadPositions.Count == 0)
				return -1;

			if (!gameState.Map.RoadAtPosition(bestRoadPositions[0][0]))
				return bestRoadPositions[0][0];
			else return bestRoadPositions[0][1];
		}

	}
}
