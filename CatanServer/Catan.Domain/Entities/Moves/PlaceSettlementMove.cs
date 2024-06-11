using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities.Moves
{
	public class PlaceSettlementMove : Move
	{
		public PlaceSettlementMove(Player player, int position) : base(player)
		{
			Position = position;
		}

		public int Position { get; set; }

		public override ValidationResult IsValid(GameSession gameSession)
		{
			var isInitialPhase = (gameSession.Round == 1 || gameSession.Round == 2);

			if (Player != gameSession.GetTurnPlayer())
				return ValidationResult.Failure("It is not your turn");

			if (Position >= GameMapData.SETTLEMENTS_NO || Position < 0)
				return ValidationResult.Failure("Incorrect settlement index");

			if (gameSession.GameMap.Buildings[Position] != null)
				return ValidationResult.Failure("Game piece already placed here");

			if (!isInitialPhase)
			{
				var playerRoads = Player.Roads;
				bool settlementIsConnected = false;
				foreach (var road in playerRoads)
				{
					var (roadEnd1, roadEnd2) = GameMapData.RoadEnds[road.Position];
					if (roadEnd1 == Position || roadEnd2 == Position)
					{
						settlementIsConnected = true;
						break;
					}
				}
				if (!settlementIsConnected)
					return ValidationResult.Failure("Settlement has no adjacent road");


				if (!Player.HasResources(Buyable.SETTLEMENT))
					return ValidationResult.Failure("You do not have enough resources");

				if (Player.Settlements.Count >= GameInfo.SETTLEMENTS_PER_PLAYER)
					return ValidationResult.Failure("You have placed all your settlements.");
			}
			else if (Player.Settlements.Count >= gameSession.Round)
				return ValidationResult.Failure("You cannot place another settlement now");

			var adjacentSettlements = GameMapData.AdjacentSettlements[Position];
			foreach (var adjacentSettlement in adjacentSettlements)
			{
				if (gameSession.GameMap.Buildings[adjacentSettlement] is not null)
					return ValidationResult.Failure("Other settlement too close by");
			}

			return ValidationResult.Success();
		}
	}
}
