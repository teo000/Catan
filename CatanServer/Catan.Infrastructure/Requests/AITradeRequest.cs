using Catan.Application.Dtos;

namespace Catan.Infrastructure.Requests;

public class AITradeRequest
{
	public GameSessionDto GameState { get; set; }
	public Guid PlayerId { get; set; }
	public TradeDto Trade { get; set; }
}
