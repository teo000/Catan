using Catan.Domain.Common;
using Catan.Domain.Entities;

namespace Catan.Application
{
	public interface IGameSessionManager
	{
		Result<GameSession> CreateGameSession(List<Player> players);
		Result<GameSession> RemoveGameSession(Guid guid);
		Result<GameSession> GetGameSession(Guid guid);
	}
}
