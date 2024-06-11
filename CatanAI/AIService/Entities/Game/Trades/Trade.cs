using AIService.Entities.Data;

namespace AIService.Entities.Game.Trades
{
    public class Trade
    {
		public Guid Id { get; set; }
		public Player PlayerToGive { get; set; }
		public Resource ResourceToGive { get; set; }
		public int CountToGive { get; set; }
		public Resource ResourceToReceive { get; set; }
		public int CountToReceive { get; set; }
	}
}