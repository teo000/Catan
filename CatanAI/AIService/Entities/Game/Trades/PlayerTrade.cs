using AIService.Entities.Data;

namespace AIService.Entities.Game.Trades
{
    public class PlayerTrade : Trade
    {
        public Guid PlayerToReceiveId { get; set; }
        public TradeStatus Status { get; set; } = TradeStatus.Pending;
    }
}