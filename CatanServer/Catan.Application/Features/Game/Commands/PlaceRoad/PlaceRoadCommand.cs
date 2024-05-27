using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Commands.PlaceRoad
{
    public class PlaceRoadCommand : IRequest<RoadResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
		public int Position { get; set; }
	}
}
