using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities.GamePieces;
using Catan.Domain.Entities.Harbors;
using Catan.Domain.Entities.Misc;
using Catan.Domain.Entities.Trades;
using Catan.Domain.Entities.GameMap;
using Catan.Domain.Interfaces;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Catan.Domain.Entities
{
    public class GameSession
	{
		public GameSession(List<Player> players) {
			Id = Guid.NewGuid();
			GameMap = new Map();
			Players = new List<Player>(players);

			var colors = new List<Color>() { Color.RED, Color.GREEN, Color.BLUE, Color.YELLOW};
			colors.Shuffle();
			for (int i = 0; i < players.Count; i++)
			{
				players[i].Color = colors[i];
			}

			GameStatus = GameStatus.InProgress;
			TurnPlayerIndex = 0;
			TurnEndTime = DateTime.Now.AddSeconds(GameInfo.TURN_DURATION);
			Dice = new DiceRoll();

			developmentCardsLeft.Add(DevelopmentType.VICTORY_POINT, 5);
			developmentCardsLeft.Add(DevelopmentType.KNIGHT, 14);
		}
		public Guid Id { get; private set; }
		public Map GameMap { get; private set; }
		public List<Player> Players { get; private set; }
		public GameStatus GameStatus { get; private set; }
		public int TurnPlayerIndex { get; private set; }
		public DateTime TurnEndTime { get; private set; }
		public int Round { get; private set; } = 1;
		public DiceRoll Dice { get; private set; }
		public List<PlayerTrade> Trades { get; private set; } = new List<PlayerTrade>();
		public Player? Winner { get; private set; }
		public LongestRoad LongestRoad { get; private set; }
		public bool ThiefMovedThisTurn { get; private set; } = false;
		public string Message { get; set; }

		private Dictionary<DevelopmentType, int> developmentCardsLeft = new Dictionary<DevelopmentType, int>();
		private static Random rng = new Random();

		public static Result<GameSession> Create (List<Player> players)
		{
			//if (players == null || players.Count < 3 || players.Count > 4)
			//	return Result<GameSession>.Failure("Not enough players.");
			if (players == null || players.Count < 2)
				return Result<GameSession>.Failure("Not enough players.");
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
			{
				if (IsInBeginningPhase())
					Players.Reverse();

				Round++;

				if (Round == 3)
					AssignBeginningResources();
			}


			Dice.Reset();
			ThiefMovedThisTurn = false;

			foreach(Player player in Players)
			{
				player.SetDiscardedThisTurnFalse();
			}

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

			if (GameMap.Buildings[position] != null)
				return Result<Settlement>.Failure("Game piece already placed here");

			if (!isInitialPhase) 
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

				if (player.Settlements.Count >= GameInfo.SETTLEMENTS_PER_PLAYER)
					return Result<Settlement>.Failure("You have placed all your settlements.");
			}
			else if (player.Settlements.Count >= Round)
				return Result<Settlement>.Failure("You cannot place another settlement now");

			var adjacentSettlements = GameMapData.AdjacentSettlements[position];
			foreach (var adjacentSettlement in adjacentSettlements)
			{
				if (GameMap.Buildings[adjacentSettlement] is not null)
					return Result<Settlement>.Failure("Other settlement too close by");
			}

			var newSettlement = new Settlement(player, position);


			GameMap.Buildings[position] = newSettlement;
			player.AddSettlement(newSettlement);
			if (!isInitialPhase)
				player.SubtractResources(Buyable.SETTLEMENT);
			if (GameMapData.HarbourNextToSettlement.TryGetValue(position, out var harborPosition))
			{
				var harbor = GameMap.Harbors[harborPosition];
				if (harbor is SpecialHarbor specialHarbor)
				{
					player.SetTradeCountSpecialPort(specialHarbor.Resource);
				}
				else player.SetTradeCountGeneralPort();
			}

			foreach (var hexTilePos in GameMapData.SettlementAdjacentTiles[position])
			{
				var hexTile = GameMap.HexTiles[hexTilePos];
				hexTile.Buildings.Add(newSettlement);
			}

			var winner = CheckIfIsWon();
			if (winner != null)
				MarkFinished(winner);

			return Result<Settlement>.Success(newSettlement);
		}

		//tb curatat aici dar nu acum
		public Result<Road> PlaceRoad(Player player, int position)
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
			var settlement1 = GameMap.Buildings[roadEnd1];
			var settlement2 = GameMap.Buildings[roadEnd2];

			bool connected = false;

			foreach (var road in GameMap.Roads)
				if(road is not null && road.BelongsTo(player))
				{
					var (otherRoadEnd1, otherRoadEnd2) = GameMapData.RoadEnds[road.Position];
					if (otherRoadEnd1 == roadEnd1 || otherRoadEnd1 == roadEnd2
						|| otherRoadEnd2 == roadEnd1 || otherRoadEnd2 == roadEnd2)
					{
						connected = true;
						break;
					}
				}

			if ( !connected && (settlement1 is null || !settlement1.BelongsTo(player))
				&& (settlement2 is null || !settlement2.BelongsTo(player)) )
				return Result<Road>.Failure("Road is not connected");


			if (isInitialPhase)
			{
				if (roadEnd1 != player.LastPlacedSettlementPos && roadEnd2 != player.LastPlacedSettlementPos)
					return Result<Road>.Failure("Road must be attached to the last placed settlement.");

				if (player.Roads.Count > Round)
					return Result<Road>.Failure("You cannot place another road now.");
			}

			if (!isInitialPhase && !player.HasResources(Buyable.ROAD))
				return Result<Road>.Failure("You do not have enough resources");

			var newRoad = new Road(player, position);

			GameMap.Roads[position] = newRoad;
			player.Roads.Add(newRoad);
			if(!isInitialPhase)
				player.SubtractResources(Buyable.ROAD);

			CalculateNewLongestRoad();

			return Result<Road>.Success(newRoad);
		}
	
		public Result<City> PlaceCity(Player player, int position)
		{
			if (player != GetTurnPlayer())
				return Result<City>.Failure("It is not your turn");

			if (!player.IsActive)
				return Result<City>.Failure("You have been kicked out of the lobby");

			if (position >= GameMapData.SETTLEMENTS_NO || position < 0)
				return Result<City>.Failure("Incorrect settlement index");

			if (GameMap.Buildings[position] == null)
				return Result<City>.Failure("You must place a settlement first");

			if (GameMap.Buildings[position] is City)
				return Result<City>.Failure("A city already exists here.");

			if (!GameMap.Buildings[position].BelongsTo(player))
				return Result<City>.Failure("You cannot place a city over your opponent's.");

			if (!player.HasResources(Buyable.CITY))
				return Result<City>.Failure("You do not have enough resources");

			if (player.Cities.Count >= GameInfo.CITIES_PER_PLAYER)
				return Result<City>.Failure("You have placed all your cities.");

			var newCity = new City(player, position);

			GameMap.Buildings[position] = newCity;
			player.Cities.Add(newCity);
			player.SubtractResources(Buyable.CITY);

			var winner = CheckIfIsWon();
			if (winner != null)
				MarkFinished(winner);

			return Result<City>.Success(newCity);

		}

		public Player? CheckIfIsWon()
		{
			foreach (var player in Players)
			{
				var points = player.CalculatePoints();
				if (LongestRoad is not null && LongestRoad.Player.Equals(player))
					points += 2;

				player.WinningPoints = points;

				points += player.DevelopmentCards
							.Where(c => c.DevelopmentType == DevelopmentType.VICTORY_POINT)
							.Count();

				if (points >= GameInfo.WINNING_POINTS)
					return player;
			}
			return null;
		}

		public Result<DiceRoll> RollDice (Player player)
		{
			if (player != GetTurnPlayer())
				return Result<DiceRoll>.Failure("It is not your turn.");
			if (Dice.RolledThisTurn)
				return Result<DiceRoll>.Failure("The dice can only be rolled once a turn.");
			if (IsInBeginningPhase())
				return Result<DiceRoll>.Failure("The dice can't be rolled right now.");

			Dice.Roll();
			var assignedResources = AssignResources(Dice.GetSummedValue());
			
			return Result<DiceRoll>.Success(Dice);
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
					foreach (var building in hexTile.Buildings)
						if (GameMap.ThiefPosition != building.Position)
						{
							var player = building.Player;
							var resource = hexTile.Resource;
							var count = building.Points;

							if (!resourcesToAdd[player].ContainsKey(resource))
								resourcesToAdd[player].Add(resource, count);
							else resourcesToAdd[player][resource] += count;
						}

			foreach (var player in Players)
				player.AssignResources(resourcesToAdd[player]);

			return resourcesToAdd;
		}

		private void AssignBeginningResources()
		{
			var hexTiles = GameMap.HexTiles;
			Dictionary<Player, Dictionary<Resource, int>> resourcesToAdd = new Dictionary<Player, Dictionary<Resource, int>>();

			foreach (var player in Players)
			{
				resourcesToAdd.Add(player, new Dictionary<Resource, int>());
			}

			foreach (var hexTile in hexTiles)
				foreach (var building in hexTile.Buildings)
				{
					var player = building.Player;
					var resource = hexTile.Resource;
					var count = building.Points;

					if (!resourcesToAdd[player].ContainsKey(resource))
						resourcesToAdd[player].Add(resource, count);
					else 
						resourcesToAdd[player][resource] += count;
				}

			foreach (var player in Players)
				player.AssignResources(resourcesToAdd[player]);


		}

		public void MarkAbandoned()
		{
			GameStatus = GameStatus.Abandoned;
			foreach(var player in Players)
			{
				player.Kick();
			}
		}

		public void MarkFinished(Player player)
		{
			GameStatus = GameStatus.Finished;
			Winner = player;
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

		public Result<PlayerTrade> AddNewPendingTrade(Player playerToGive, Resource resourceToGive, int countToGive, Player playerToReceive, Resource resourceToReceive, int countToReceive)	
		{
			var tradeResult = PlayerTrade.Create(playerToGive, resourceToGive, countToGive, playerToReceive, resourceToReceive, countToReceive);	

			if (!tradeResult.IsSuccess) 
				return tradeResult;

			var trade = tradeResult.Value;
			Trades.Add(trade);

			return tradeResult;
		}
		
		public Result<PlayerTrade> GetTrade(Guid TradeId)
		{
			var trade = getTrade(TradeId);
			if (trade is not null)
				return Result<PlayerTrade>.Success(trade);
			return Result<PlayerTrade>.Failure("Trade does not exist in current context.");
		}

		public Result<PlayerTrade> RespondToTrade(Guid TradeId, bool accepted)
		{
			var trade = getTrade(TradeId);

			if (trade.Status != TradeStatus.Pending)
				return Result<PlayerTrade>.Failure("Trade is no longer available.");

			if (trade is null)
				return Result<PlayerTrade>.Failure("Trade does not exist in current context.");

			if (accepted && !trade.PlayerToReceive.HasResource(trade.ResourceToReceive, trade.CountToReceive))
				return Result<PlayerTrade>.Failure("You do not have enough resources");

			if (accepted && !trade.PlayerToGive.HasResource(trade.ResourceToGive, trade.CountToGive))
				return Result<PlayerTrade>.Failure("Trade could not be completed");

			if (!accepted)
			{
				trade.SetRejected();
				return Result<PlayerTrade>.Success(trade);
				
			}

			trade.PlayerToReceive.SubtractResource(trade.ResourceToReceive, trade.CountToReceive);
			trade.PlayerToGive.SubtractResource(trade.ResourceToGive, trade.CountToGive);

			trade.PlayerToReceive.AssignResource(trade.ResourceToGive, trade.CountToGive);
			trade.PlayerToGive.AssignResource(trade.ResourceToReceive, trade.CountToReceive);

			trade.SetAccepted();

			return Result<PlayerTrade>.Success(trade);

		}

		public Result<GameSession> TradeBank(Player player, Resource resourceToGive, int count, Resource resourceToReceive)
		{
			var countToTrade = count * player.TradeCount[resourceToGive];

			if (player != GetTurnPlayer())
				return Result<GameSession>.Failure("It is not your turn.");
			if (!player.HasResource(resourceToGive, countToTrade))
				return Result<GameSession>.Failure("You do not have enough resources");

			player.SubtractResource(resourceToGive, countToTrade);
			player.AssignResource(resourceToReceive, count);

			return Result<GameSession>.Success(this);
		}


		public Result<Map> MoveThief(Player player, int position)
		{
			if (player != GetTurnPlayer())
				return Result<Map>.Failure("It is not your turn.");

			if (Dice.GetSummedValue() != 7)
				return Result<Map>.Failure("You have to roll a 7 to move the thief,");

			if (GameMap.ThiefPosition == position)
				return Result<Map>.Failure("Move the thief to a different position.");

			if (ThiefMovedThisTurn == true)
				return Result<Map>.Failure("Thief already moved this turn");

			GameMap.MoveThief(position);
			ThiefMovedThisTurn = true;
			return Result<Map>.Success(GameMap);
		}

		public Result<Dictionary<Resource, int>> DiscardHalf(Player player, Dictionary<Resource, int> toDiscard)
		{
			if (!Dice.RolledThisTurn || !(Dice.GetSummedValue() == 7))
				return Result<Dictionary<Resource, int>>.Failure("Dice must land on a 7 to discard half of your cards.");

			if (player.DiscardedThisTurn)
				return Result<Dictionary<Resource, int>>.Failure("You have already discarded half of your cards this round.");

			if (!player.HasResources(toDiscard))
			{
				return Result<Dictionary<Resource, int>>.Failure($"You do not have those resources: {GameUtils.PrintDictionary(player.ResourceCount)}");
			}


			player.DiscardHalf(toDiscard);
			return Result<Dictionary<Resource, int>>.Success(player.ResourceCount);
		}

		public void CalculateNewLongestRoad()
		{
			//adauga caz special broken road
			foreach (var player in Players) {
				var roadEnds = GameMap.Roads
					.Where(r => r is not null && r.BelongsTo(player))
					.Select(r => GameMapData.RoadEnds[r.Position]).ToList();

				var (longestRoadLength, longestRoad) = GraphFunctions.FindLongestRoad(roadEnds);

				if (longestRoadLength >= 5 && longestRoadLength >= LongestRoad.Roads.Count)
				{
					var roadList = longestRoad.Select(r => GameMapData.RoadByRoadEnds[r])
						.Select(pos => GameMap.Roads[pos]).ToList();
					LongestRoad = new LongestRoad(roadList, player);
				}
			}

		}

		private PlayerTrade getTrade(Guid id)
		{
			foreach (var trade in Trades)
			{
				if (trade.Id == id) return trade;
			}
			return null;
		}

		public Result<DevelopmentCard> BuyDevelopmentCard(Player player)
		{
			if (!player.HasResources(Buyable.DEVELOPMENT))
			{
				return Result<DevelopmentCard>.Failure($"You do not have those resources: {GameUtils.PrintDictionary(player.ResourceCount)}");
			}

			var allCards = developmentCardsLeft.SelectMany(kvp => Enumerable.Repeat(kvp.Key, kvp.Value)).ToList();

			if (allCards.Count == 0)
			{
				return Result<DevelopmentCard>.Failure("No development cards left to draw.");
			}

			int randomIndex = rng.Next(allCards.Count);

			DevelopmentType selectedCard = allCards[randomIndex];
			developmentCardsLeft[selectedCard]--;

			var newDevelopmentCard = new DevelopmentCard(selectedCard, Round);

			player.AddDevelopmentCard(newDevelopmentCard);

			return Result<DevelopmentCard>.Success(newDevelopmentCard);
		}
	}
}
