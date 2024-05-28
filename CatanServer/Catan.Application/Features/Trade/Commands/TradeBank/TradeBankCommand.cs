using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.TradeBank
{
	public class TradeBankCommand : IRequest<BaseResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
		public string ResourceToGive {  get; set; }
		public string ResourceToReceive { get; set; }
		public int Count { get; set; }
	}
}
