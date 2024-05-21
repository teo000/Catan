using Catan.Application.Features.Game.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Commands.PlaceSettlement
{
	public class PlaceSettlementCommand : IRequest<SettlementResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
		public int Position { get; set; }
	}
}
