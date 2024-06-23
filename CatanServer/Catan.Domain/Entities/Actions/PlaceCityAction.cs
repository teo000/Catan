using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities.Moves
{
	public class PlaceCityAction : Action<City>
	{
		public int Position { get; }

		public PlaceCityAction(Player player, int position) : base(player)
		{
			Position = position;
		}

		public override ValidationResult IsValid(GameSession gameSession)
		{
			if (Player != gameSession.GetTurnPlayer())
				return ValidationResult.Failure("It is not your turn");

			if (!Player.IsActive)
				return ValidationResult.Failure("You have been kicked out of the lobby");

			if (Position >= GameMapData.SETTLEMENTS_NO || Position < 0)
				return ValidationResult.Failure("Incorrect settlement index");

			if (gameSession.GameMap.Buildings[Position] == null)
				return ValidationResult.Failure("You must place a settlement first");

			if (gameSession.GameMap.Buildings[Position] is City)
				return ValidationResult.Failure("A city already exists here.");

			if (!gameSession.GameMap.Buildings[Position].BelongsTo(Player))
				return ValidationResult.Failure("You cannot place a city over your opponent's.");

			if (!Player.HasResources(Buyable.CITY))
				return ValidationResult.Failure("You do not have enough resources");

			if (Player.Cities.Count >= GameInfo.CITIES_PER_PLAYER)
				return ValidationResult.Failure("You have placed all your cities.");

			return ValidationResult.Success();
		}

		public override City Execute(GameSession gameSession)
		{
			var newCity = new City(Player, Position);

			gameSession.GameMap.Buildings[Position] = newCity;
			Player.Cities.Add(newCity);
			Player.SubtractResources(Buyable.CITY);

			var winner = gameSession.CheckIfIsWon();
			if (winner != null)
				gameSession.MarkFinished(winner);

			return newCity;
		}
	}
}