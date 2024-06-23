using Catan.Application.Dtos;
using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Commands.MakeMove
{
	public class MakeMoveCommand : IRequest<GameSessionResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
		public string MoveType { get; set; }
		public int? Position { get; set; }
	}
}
