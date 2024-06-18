using Catan.Application.Dtos;
using Catan.Application.Moves;
using Catan.Domain.Common;

namespace Catan.Application.Contracts
{
	public interface IAIService
	{
		public Task<Result<List<Move>>> MakeAIMove(GameSessionDto gameSession);
	}
}
