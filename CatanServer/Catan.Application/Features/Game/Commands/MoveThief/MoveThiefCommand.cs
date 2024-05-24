using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Commands.MoveThief
{
	public class MoveThiefCommand : IRequest<MapResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
		public int Position { get; set; }
	}
}
