using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.CommandsObsolete.PlaceSettlement
{
    public class PlaceSettlementCommand : IRequest<SettlementResponse>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int Position { get; set; }
    }
}
