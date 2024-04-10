using Catan.Domain.Common;
using Catan.Domain.Entities;

namespace Catan.Application
{
	public class GameSessionManager : IGameSessionManager
	{
		private Dictionary<Guid, GameSession> gameSessions = new Dictionary<Guid, GameSession>();

		public Result<GameSession> CreateGameSession(List<Player> players)
		{
			var result = GameSession.Create(players);
			if (result.IsSuccess)
			{
				var game = result.Value;
				gameSessions.Add(game.Id, game);
			}
			return result;
		}

		public Result<GameSession> GetGameSession(Guid guid)
		{
			if (gameSessions.TryGetValue(guid, out var game))
			{
				return Result<GameSession>.Success(game);
			}
			else
			{
				return Result<GameSession>.Failure("Game does not exist in current context");
			}
		}

		public Result<GameSession> RemoveGameSession(Guid guid)
		{
			if (gameSessions.TryGetValue(guid, out var game))
			{
				gameSessions.Remove(guid);
				return Result<GameSession>.Success(game);
			}
			else
			{
				return Result<GameSession>.Failure("Game does not exist in current context");
			}
		}
	}
}
