using CatanGame.Common;
using CatanGame.Data;

namespace CatanGame.Entities
{
	public class GameSession
	{
		public GameSession(List<Player> players) {
			Id = Guid.NewGuid();
			GameMap = new Map();
			Players = new List<Player>(players);
			GameStatus = GameStatus.InProgress;
			TurnPlayerIndex = 0;
		}
		public Guid Id { get; private set; }
		public Map GameMap { get; private set; }
		public List<Player> Players { get; private set; }
		public GameStatus GameStatus { get; private set; }
		public int TurnPlayerIndex { get; private set; }
		public Player GetTurnPlayer(){
			return Players[TurnPlayerIndex];
		}

		public static Result<GameSession> Create (List<Player> players)
		{
			//if (players == null || players.Count < 3 || players.Count > 4)
			//	return Result<GameSession>.Failure("Not enough players.");
			return Result<GameSession>.Success(new GameSession(players));
		}

		public void ChangeTurn()
		{
			TurnPlayerIndex = (TurnPlayerIndex + 1) % Players.Count;
		}

		//astea is ditai clasele vreau sa le fac mai ok ca ma doare capul
		public Result<Settlement> PlaceSettlement(int position, bool isInitialPhase = false)
		{
			var turnPlayer = GetTurnPlayer();

			if (position >= GameMapData.SETTLEMENTS_NO || position < 0)
				return Result<Settlement>.Failure("Incorrect settlement index");

			if (GameMap.Settlements[position] != null)
				return Result<Settlement>.Failure("Settlement already placed here");

			if (!isInitialPhase) //netestat
			{
				var playerRoads = turnPlayer.Roads;
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


				if (!turnPlayer.HasResources(Buyables.SETTLEMENT))
					return Result<Settlement>.Failure("You do not have enough resources");
			}

			

			var adjacentSettlements = GameMapData.AdjacentSettlements[position];
			foreach (var adjacentSettlement in adjacentSettlements)
			{
				if (GameMap.Settlements[adjacentSettlement] is not null)
					return Result<Settlement>.Failure("Other settlement too close by");
			}

			var newSettlement = new Settlement(GetTurnPlayer(), false, position);

			GameMap.Settlements[position] = newSettlement;
			GetTurnPlayer().Settlements.Add(newSettlement);

			return Result<Settlement>.Success(newSettlement);
		}

		public Result<Road> PlaceRoad(int position, bool isInitialPhase = false, int? lastPlacedSettlementPos = null)
		{
			var turnPlayer = GetTurnPlayer();

			if (position >= GameMapData.ROADS_NO)
				return Result<Road>.Failure("Incorrect road index");

			if (GameMap.Roads[position] != null)
				return Result<Road>.Failure("Road already placed here");

			
			var (roadEnd1, roadEnd2) = GameMapData.RoadEnds[position];
			var settlement1 = GameMap.Settlements[roadEnd1];
			var settlement2 = GameMap.Settlements[roadEnd2];

			if ( (settlement1 is null || !settlement1.BelongsTo(turnPlayer))
				&& (settlement2 is null || !GameMap.Settlements[roadEnd2].BelongsTo(turnPlayer)) )
				return Result<Road>.Failure("Road is not connected to any settlement");


			if (isInitialPhase && roadEnd1 != lastPlacedSettlementPos && roadEnd2 != lastPlacedSettlementPos)
				return Result<Road>.Failure("Road must be attached to the last placed settlement");
				
			if (!isInitialPhase && !turnPlayer.HasResources(Buyables.ROAD))
				return Result<Road>.Failure("You do not have enough resources");

			var newRoad = new Road(GetTurnPlayer(), position);

			GameMap.Roads[position] = newRoad;
			GetTurnPlayer().Roads.Add(newRoad);

			return Result<Road>.Success(newRoad);
		}
	}
}
