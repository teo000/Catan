using AIService.Entities.Common;
using AIService.Entities.Moves;

namespace AIService.Interfaces
{
	public interface IGameBackendService
	{
		Task<Result<Move>> NotifyMoveAsync(Move move);
		//Task<GameState> GetGameStateAsync(Guid gameId);
	}
}
