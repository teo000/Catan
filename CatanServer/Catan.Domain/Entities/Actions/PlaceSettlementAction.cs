using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities.GamePieces;
using Catan.Domain.Entities.Harbors;

namespace Catan.Domain.Entities.Moves
{
	public class PlaceSettlementAction : Action<Settlement>
	{
		public PlaceSettlementAction(Player player, int position) : base(player)
		{
			Position = position;
		}

		public int Position { get; set; }

		public override Settlement Execute(GameSession gameSession)
		{
			var newSettlement = new Settlement(Player, Position);

			gameSession.GameMap.Buildings[Position] = newSettlement;
			Player.AddSettlement(newSettlement);
			if (gameSession.Round > 2)
				Player.SubtractResources(Buyable.SETTLEMENT);
			if (GameMapData.HarbourNextToSettlement.TryGetValue(Position, out var harborPosition))
			{
				var harbor = gameSession.GameMap.Harbors[harborPosition];
				if (harbor is SpecialHarbor specialHarbor)
				{
					Player.SetTradeCountSpecialPort(specialHarbor.Resource);
				}
				else Player.SetTradeCountGeneralPort();
			}

			foreach (var hexTilePos in GameMapData.SettlementAdjacentTiles[Position])
			{
				var hexTile = gameSession.GameMap.HexTiles[hexTilePos];
				hexTile.Buildings.Add(newSettlement);
			}

			var winner = gameSession.CheckIfIsWon();
			if (winner != null)
				gameSession.MarkFinished(winner);

			return newSettlement;
		}

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
