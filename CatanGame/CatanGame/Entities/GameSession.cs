using CatanGame.Common;
using CatanGame.Data;

namespace CatanGame.Entities
{
	public class GameSession
	{
		public GameSession(List<Player> players) {
			Id = Guid.NewGuid();
			GameMap = new Map();
			Players = players;
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
			if (players == null || players.Count < 4)
				return Result<GameSession>.Failure("Not enough players.");
			return Result<GameSession>.Success(new GameSession(players));
		}

		public void ChangeTurn()
		{
			TurnPlayerIndex = (TurnPlayerIndex + 1) % Players.Count;
		}

		public Result<Settlement> PlaceSettlement(int position, bool isCity = false, bool isBeginning = false)
		{
			if (position >= GameMapData.SETTLEMENTS_NO)
				return Result<Settlement>.Failure("Incorrect settlement index");

			if (GameMap.Settlements[position] != null)
				return Result<Settlement>.Failure("Settlement already placed here");

			var adjacentSettlements = GameMapData.AdjacentSettlements[position];
			foreach (var adjacentSettlement in adjacentSettlements)
			{
				if (GameMap.Settlements[adjacentSettlement] is not null)
					return Result<Settlement>.Failure("Other settlement too close by");
			}

			var newSettlement = new Settlement(GetTurnPlayer(), isCity);

			GameMap.Settlements[position] = newSettlement;
			GetTurnPlayer().Settlements.Add(newSettlement);

			return Result<Settlement>.Success(newSettlement);
		}

		public Result<Road> PlaceRoad(int position)
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

			var newRoad = new Road(GetTurnPlayer());

			GameMap.Roads[position] = newRoad;
			GetTurnPlayer().Roads.Add(newRoad);

			return Result<Road>.Success(newRoad);
		}
	}
}
