using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities.GamePieces;

namespace Catan.Domain.Entities.Moves
{
	public class PlaceRoadAction : Action<Road>
	{
		public PlaceRoadAction(Player player, int position) : base(player)
		{
			Position = position;
		}

		public int Position { get; }

		public override Road Execute(GameSession gameSession)
		{
			var newRoad = new Road(Player, Position);

			gameSession.GameMap.Roads[Position] = newRoad;
			Player.Roads.Add(newRoad);
			if (!(gameSession.Round == 1 || gameSession.Round == 2))
				Player.SubtractResources(Buyable.ROAD);

			gameSession.CalculateNewLongestRoad();

			return newRoad;
		}

		public override ValidationResult IsValid(GameSession gameSession)
		{
			var isInitialPhase = (gameSession.Round == 1 || gameSession.Round == 2);

			if (Player != gameSession.GetTurnPlayer())
				return ValidationResult.Failure("It is not your turn");

			if (!Player.IsActive)
				return ValidationResult.Failure("You have been kicked out of the lobby");

			if (Position >= GameMapData.ROADS_NO)
				return ValidationResult.Failure("Incorrect road index");

			if (gameSession.GameMap.Roads[Position] != null)
				return ValidationResult.Failure("Road already placed here");

			var (roadEnd1, roadEnd2) = GameMapData.RoadEnds[Position];
			var settlement1 = gameSession.GameMap.Buildings[roadEnd1];
			var settlement2 = gameSession.GameMap.Buildings[roadEnd2];

			bool connected = false;

			foreach (var road in gameSession.GameMap.Roads)
			{
				if (road is not null && road.BelongsTo(Player))
				{
					var (otherRoadEnd1, otherRoadEnd2) = GameMapData.RoadEnds[road.Position];
					if (otherRoadEnd1 == roadEnd1 || otherRoadEnd1 == roadEnd2 || otherRoadEnd2 == roadEnd1 || otherRoadEnd2 == roadEnd2)
					{
						connected = true;
						break;
					}
				}
			}

			if (!connected && (settlement1 is null || !settlement1.BelongsTo(Player)) && (settlement2 is null || !settlement2.BelongsTo(Player)))
				return ValidationResult.Failure("Road is not connected");

			if (isInitialPhase)
			{
				if (roadEnd1 != Player.LastPlacedSettlementPos && roadEnd2 != Player.LastPlacedSettlementPos)
					return ValidationResult.Failure("Road must be attached to the last placed settlement.");

				if (Player.Roads.Count > gameSession.Round)
					return ValidationResult.Failure("You cannot place another road now.");
			}

			if (!isInitialPhase && !Player.HasResources(Buyable.ROAD))
				return ValidationResult.Failure("You do not have enough resources");

			return ValidationResult.Success();
		}

	}
}
