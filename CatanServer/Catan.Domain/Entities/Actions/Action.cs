using Catan.Domain.Common;

namespace Catan.Domain.Entities.Moves
{
	public abstract class Action<T>
	{
		public Action (Player player)
		{
			Player = player;
		}

		public Player Player { get; set; }
		public abstract ValidationResult IsValid(GameSession gameSession);
		public abstract T Execute(GameSession gameSession);
	}
}
