using Catan.Domain.Common;

namespace Catan.Domain.Entities
{
	public class Lobby
	{
		public Lobby(string joinCode, Player player)
		{
			Id = Guid.NewGuid();
			JoinCode = joinCode;
			Players.Add(player);
		}
		public Guid Id { get; private set; }
		public string JoinCode { get; private set; }
		public List<Player> Players { get; private set; } = new List<Player>();
		public GameSession? GameSession { get; private set; }

		public const int MAX_PLAYERS = 4;
		
		public void SetGameSession(GameSession gameSession)
		{
			GameSession = gameSession;
		}

		public Result<Lobby> Join(Player player)
		{
			if (Players.Count >= MAX_PLAYERS)
				return Result<Lobby>.Failure("This lobby is already full.");
			if (Players.Contains(player))
				return Result<Lobby>.Failure("You have already joined this lobby.");
			Players.Add(player);
			return Result<Lobby>.Success(this);
		}

		public Result<Lobby> Leave(Player player) 
		{
			if (!Players.Contains(player))
				return Result<Lobby>.Failure("You are not in this lobby");
			Players.Remove(player);
			return Result<Lobby>.Success(this);
		}


	}
}
