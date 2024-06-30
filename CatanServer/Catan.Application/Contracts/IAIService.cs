using Catan.Application.Dtos;
using Catan.Application.Models.Moves;
using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using Catan.Domain.Entities.Trades;

namespace Catan.Application.Contracts
{
    public interface IAIService
	{
		public Task<Result<List<Move>>> MakeAIMove(GameSession gameSession, Guid playerId);
		public Task<Result<Dictionary<Resource, int>>> DiscardHalfOfResources (GameSession gameSession, Guid playerId);
		public Task<Result<bool>> RespondToTrade (GameSession gameSession, Guid playerId, Trade trade);

	}
}
