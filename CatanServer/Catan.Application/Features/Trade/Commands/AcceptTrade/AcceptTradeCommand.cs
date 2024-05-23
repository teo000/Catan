using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.AcceptTrade
{
	public class AcceptTradeCommand : IRequest<TradeResponse>
	{
		public Guid GameId { get; set; }
		public Guid TradeId { get; set;}
		public Guid PlayerId { get; set; }
	}
}
