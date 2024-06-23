using AIService.Entities.Data;

namespace AIService.Entities.Game.Trades
{
    public class PlayerTrade : Trade
    {
        public Guid PlayerToReceiveId { get; private set; }
        public TradeStatus Status { get; private set; } = TradeStatus.Pending;
    }
}