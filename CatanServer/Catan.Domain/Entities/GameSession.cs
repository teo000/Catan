using Catan.Domain.Common;
using Catan.Domain.Data;
using System.Linq.Expressions;

namespace Catan.Domain.Entities
{
	public class GameSession
	{
		public GameSession(List<Player> players) {
			Id = Guid.NewGuid();
			GameMap = new Map();
			Players = new List<Player>(players);
			GameStatus = GameStatus.InProgress;
			TurnPlayerIndex = 0;
			TurnEndTime = DateTime.Now.AddSeconds(GameInfo.TURN_DURATION);
			Dice = new DiceRoll();
		}
		public Guid Id { get; private set; }
		public Map GameMap { get; private set; }
		public List<Player> Players { get; private set; }
		public GameStatus GameStatus { get; private set; }
		public int TurnPlayerIndex { get; private set; }
		public DateTime TurnEndTime { get; private set; }
		public int Round { get; private set; } = 1;
		public DiceRoll Dice { get; private set; }
		public Dictionary<Guid, Trade> Trades { get; private set; } = new Dictionary<Guid, Trade>();

		public static Result<GameSession> Create (List<Player> players)
		{
			//if (players == null || players.Count < 3 || players.Count > 4)
			//	return Result<GameSession>.Failure("Not enough players.");
			return Result<GameSession>.Success(new GameSession(players));
		}

		public Player GetTurnPlayer()
		{
			return Players[TurnPlayerIndex];
		}

		public bool IsInBeginningPhase()
		{
			return (Round == 1 || Round == 2);
		}

		public void EndPlayerTurn()
		{
			TurnPlayerIndex = (TurnPlayerIndex + 1) % Players.Count;
			if (TurnPlayerIndex == 0)
				Round++;
			Dice.RolledThisTurn = false;
			TurnEndTime = DateTime.Now.AddSeconds(GameInfo.TURN_DURATION);
		}

		//astea is ditai clasele vreau sa le fac mai ok ca ma doare capul
		public Result<Settlement> PlaceSettlement(Player player, int position)
		{
			var isInitialPhase = (Round == 1 || Round == 2);

			if (player != GetTurnPlayer())
				return Result<Settlement>.Failure("It is not your turn");

			if (!player.IsActive)
				return Result<Settlement>.Failure("You have been kicked out of the lobby");

			if (position >= GameMapData.SETTLEMENTS_NO || position < 0)
				return Result<Settlement>.Failure("Incorrect settlement index");

			if (GameMap.Settlements[position] != null)
				return Result<Settlement>.Failure("Settlement already placed here");

			if (!isInitialPhase) //netestat
			{
				var playerRoads = player.Roads;
				bool settlementIsConnected = false;
				foreach (var road in playerRoads)
				{
					var (roadEnd1, roadEnd2) = GameMapData.RoadEnds[road.Position];
					if (roadEnd1 == position || roadEnd2 == position)
					{
						settlementIsConnected = true;
						break;
					}
				}
				if (!settlementIsConnected)
					return Result<Settlement>.Failure("Settlement has no adjacent road");


				if (!player.HasResources(Buyable.SETTLEMENT))
					return Result<Settlement>.Failure("You do not have enough resources");
			}
			else if (player.Settlements.Count > Round)
				return Result<Settlement>.Failure("You cannot place another settlement now");

			var adjacentSettlements = GameMapData.AdjacentSettlements[position];
			foreach (var adjacentSettlement in adjacentSettlements)
			{
				if (GameMap.Settlements[adjacentSettlement] is not null)
					return Result<Settlement>.Failure("Other settlement too close by");
			}

			//vezi si ca are destule

			var newSettlement = new Settlement(player, false, position);

			GameMap.Settlements[position] = newSettlement;
			player.Settlements.Add(newSettlement);

			return Result<Settlement>.Success(newSettlement);
		}

		//tb curatat aici dar nu acum
		public Result<Road> PlaceRoad(Player player, int position, int? lastPlacedSettlementPos = null)
		{
			var isInitialPhase = (Round == 1 || Round == 2);

			if (player != GetTurnPlayer())
				return Result<Road>.Failure("It is not your turn");

			if (!player.IsActive)
				return Result<Road>.Failure("You have been kicked out of the lobby");

			if (position >= GameMapData.ROADS_NO)
				return Result<Road>.Failure("Incorrect road index");

			if (GameMap.Roads[position] != null)
				return Result<Road>.Failure("Road already placed here");

			
			var (roadEnd1, roadEnd2) = GameMapData.RoadEnds[position];
			var settlement1 = GameMap.Settlements[roadEnd1];
			var settlement2 = GameMap.Settlements[roadEnd2];

			if ((settlement1 is null || !settlement1.BelongsTo(player))
				&& (settlement2 is null || !settlement2.BelongsTo(player)) )
				return Result<Road>.Failure("Road is not connected to any settlement");


			if (isInitialPhase)
			{
				if (lastPlacedSettlementPos == null)
					return Result<Road>.Failure("Server-side error.");

				if (roadEnd1 != lastPlacedSettlementPos && roadEnd2 != lastPlacedSettlementPos)
					return Result<Road>.Failure("Road must be attached to the last placed settlement.");

				if (player.Roads.Count > Round)
					return Result<Road>.Failure("You cannot place another road now.");
			}

			if (!isInitialPhase && !player.HasResources(Buyable.ROAD))
			return Result<Road>.Failure("You do not have enough resources");

			var newRoad = new Road(player, position);

			GameMap.Roads[position] = newRoad;
			player.Roads.Add(newRoad);

			return Result<Road>.Success(newRoad);
		}

		public Result<Settlement> PlaceCity(Player player, int position)
		{
			if (player != GetTurnPlayer())
				return Result<Settlement>.Failure("It is not your turn");

			if (!player.IsActive)
				return Result<Settlement>.Failure("You have been kicked out of the lobby");

			if (position >= GameMapData.SETTLEMENTS_NO || position < 0)
				return Result<Settlement>.Failure("Incorrect settlement index");

			if (GameMap.Settlements[position] == null)
				return Result<Settlement>.Failure("You must place a settlement first");

			if (GameMap.Settlements[position].IsCity)
				return Result<Settlement>.Failure("A city already exists here.");

			if (!GameMap.Settlements[position].BelongsTo(player))
				return Result<Settlement>.Failure("You cannot place a city over your opponent's.");

			if (!player.HasResources(Buyable.CITY))
				return Result<Settlement>.Failure("You do not have enough resources");

			//vezi si ca are destule

			var newCity = new Settlement(player, true, position);

			GameMap.Settlements[position] = newCity;
			player.Settlements.Add(newCity);

			return Result<Settlement>.Success(newCity);

		}

		public Player? CheckIfIsWon()
		{
			foreach (var player in Players)
				if (player.CalculatePoints() >= 10)
					return player;
			return null;
		}

		public Result<Dictionary<Player, Dictionary<Resource, int>>> RollDice (Player player)
		{
			if (player != GetTurnPlayer())
				return Result<Dictionary<Player, Dictionary<Resource, int>>>.Failure("It is not your turn.");
			if (Dice.RolledThisTurn)
				return Result<Dictionary<Player, Dictionary<Resource, int>>>.Failure("The dice can only be rolled once a turn.");

			Dice.Roll();
			var assignedResources = AssignResources(Dice.GetSummedValue());
			
			return Result<Dictionary<Player, Dictionary<Resource, int>>>.Success(assignedResources);
		}

		private Dictionary<Player, Dictionary<Resource, int>> AssignResources(int number)
		{
			var hexTiles = GameMap.HexTiles;
			Dictionary<Player, Dictionary<Resource, int>> resourcesToAdd = new Dictionary<Player, Dictionary<Resource, int>>();

			foreach (var player in Players) {
				resourcesToAdd.Add(player, new Dictionary<Resource, int>());
			}

			foreach (var hexTile in hexTiles)
				if (hexTile.Number == number)
					foreach (var settlement in hexTile.Settlements)
						if (GameMap.ThiefPosition != settlement.Position)
						{
							var player = settlement.Player;
							var resource = hexTile.Resource;
							var count = settlement.IsCity ? 2 : 1;

							if (!resourcesToAdd[player].ContainsKey(resource))
								resourcesToAdd[player].Add(resource, count);
							else resourcesToAdd[player][resource] += count;
						}

			foreach (var player in Players)
				player.AssignResources(resourcesToAdd[player]);

			return resourcesToAdd;
		}

		public void MarkAbandoned()
		{
			GameStatus = GameStatus.Abandoned;
			foreach(var player in Players)
			{
				player.Kick();
			}
		}

		public void MarkFinished()
		{
			GameStatus = GameStatus.Finished;
		}

		public List<Player> GetActivePlayers()
		{
			var players = new List<Player>();
			foreach (var player in Players)
			{
				if (player.IsActive)
					players.Add(player);
			}

			return players;
		}

		public Result<Trade> AddNewPendingTrade(Player playerToGive, Resource resourceToGive, int countToGive, Player playerToReceive, Resource resourceToReceive, int countToReceive)
		{
			var tradeResult = Trade.Create(playerToGive, resourceToGive, countToGive, playerToReceive, resourceToReceive, countToReceive);	

			if (!tradeResult.IsSuccess) 
				return tradeResult;

			var trade = tradeResult.Value;
			Trades.Add(trade.Id, trade);

			return tradeResult;
		}

		public Result<Trade> GetTrade(Guid TradeId)
		{
			if (Trades.TryGetValue(TradeId, out var trade))
				return Result<Trade>.Success(trade);
			return Result<Trade>.Failure("Trade does not exist in current context.");
		}

		public Result<Trade> AcceptTrade(Guid TradeId)
		{
			Trade trade = null;
			if (!Trades.TryGetValue(TradeId, out trade))
				return Result<Trade>.Failure("Trade does not exist in current context.");

			if (!trade.PlayerToReceive.HasResource(trade.ResourceToReceive, trade.CountToReceive))
				return Result<Trade>.Failure("You do not have enough resources");

			if (!trade.PlayerToReceive.HasResource(trade.ResourceToGive, trade.CountToGive))
				return Result<Trade>.Failure("Trade could not be completed");


			trade.PlayerToReceive.SubtractResource(trade.ResourceToReceive, trade.CountToReceive);
			trade.PlayerToGive.SubtractResource(trade.ResourceToGive, trade.CountToGive);

			trade.PlayerToReceive.AssignResource(trade.ResourceToGive, trade.CountToGive);
			trade.PlayerToGive.AssignResource(trade.ResourceToReceive, trade.CountToReceive);

			trade.SetAccepted();

			return Result<Trade>.Success(trade);
		}

		public Result<Map> MoveThief(Player player, int position)
		{
			if (player != GetTurnPlayer())
				return Result<Map>.Failure("It is not your turn.");

			if (Dice.GetSummedValue() != 7)
				return Result<Map>.Failure("You have to roll a 7 to move the thief,");

			if (GameMap.ThiefPosition == position)
				return Result<Map>.Failure("Move the thief to a different position.");

			GameMap.MoveThief(position);
			return Result<Map>.Success(GameMap);
		}

	}
}
