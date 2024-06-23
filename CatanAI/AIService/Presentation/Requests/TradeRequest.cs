using AIService.Entities.Game;
using AIService.Entities.Game.Trades;

namespace AIService.Presentation.Requests;

public class TradeRequest
{
	public GameState GameState { get; set; }
	public Guid PlayerId { get; set; }
	public PlayerTrade Trade { get; set; }
}
