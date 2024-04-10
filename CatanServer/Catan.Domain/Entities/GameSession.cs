using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities
{
	public class GameSession
	{
		private GameSession(List<Player> players) {
			Id = Guid.NewGuid();
			Map = new Map();
			Players = players;
			GameStatus = GameStatus.InProgress;
		}
		public Guid Id { get; private set; }
		public Map? Map { get; private set; }
		public List<Player> Players { get; private set; }
		public GameStatus GameStatus { get; private set; }

		public static Result<GameSession> Create (List<Player> players)
		{
			if (players == null || players.Count < 4)
				return Result<GameSession>.Failure("Not enough players.");
			return Result<GameSession>.Success(new GameSession(players));
		}
	}
}
