using Catan.Application.Dtos;
using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Commands.EndTurn
{
	public class EndTurnCommand : IRequest<GameSessionResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
	}
}
