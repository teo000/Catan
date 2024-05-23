using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.InitiateTrade
{
    public class InitiateTradeCommand : IRequest<TradeResponse>
    {
        public Guid GameId { get; set; }
        public Guid PlayerToGiveId { get; set; }
        public string ResourceToGive { get; set; }
        public int CountToGive { get; set; }
        public Guid PlayerToReceiveId { get; set; }
        public string ResourceToReceive { get; set; }
        public int CountToReceive { get; set; }
    }
}
