using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.RespondToTrade
{
	public class RespondToTradeCommand : IRequest<GameSessionResponse>
	{
		public Guid GameId { get; set; }
		public Guid TradeId { get; set;}
		public Guid PlayerId { get; set; }
		public bool IsAccepted { get; set; }
	}
}
