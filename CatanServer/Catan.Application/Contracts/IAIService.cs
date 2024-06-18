using Catan.Application.Dtos;
using Catan.Application.Moves;
using Catan.Domain.Common;
using Catan.Domain.Entities;

namespace Catan.Application.Contracts
{
	public interface IAIService
	{
		public Task<Result<List<Move>>> MakeAIMove(GameSession gameSession, Guid playerId);
	}
}
