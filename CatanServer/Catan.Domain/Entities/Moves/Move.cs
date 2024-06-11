using Catan.Domain.Common;

namespace Catan.Domain.Entities.Moves
{
	public abstract class Move
	{
		public Move (Player player)
		{
			Player = player;
		}

		public Player Player { get; set; }
		public abstract ValidationResult IsValid(GameSession gameSession);
	}
}
