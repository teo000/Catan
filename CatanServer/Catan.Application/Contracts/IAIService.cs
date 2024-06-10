using Catan.Application.Dtos;
using Catan.Domain.Common;

namespace Catan.Application.Contracts
{
	public interface IAIService
	{
		public Task<Result<bool>> MakeAIMove(GameSessionDto gameSession);
	}
}
