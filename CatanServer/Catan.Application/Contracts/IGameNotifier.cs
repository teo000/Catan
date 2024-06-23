using Catan.Application.Dtos;
using Catan.Domain.Entities;

namespace Catan.Application.Contracts
{
	public interface IGameNotifier
	{
		Task NotifyGameAsync(GameSessionDto session);
	}
}
