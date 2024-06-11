using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities.Trades
{
	public class PlayerTrade : Trade
	{
		private PlayerTrade(Player playerToGive, Resource resourceToGive, int countToGive, Player playerToReceive, Resource resourceToReceive, int countToReceive)
			: base(playerToGive, resourceToGive, countToGive, resourceToReceive, countToReceive)
		{
			PlayerToReceive = playerToReceive;	
		}

		public Player PlayerToReceive { get; }
		public TradeStatus Status { get; private set; } = TradeStatus.Pending;

		public static Result<PlayerTrade> Create(Player playerToGive, Resource resourceToGive, int countToGive, Player playerToReceive, Resource resourceToReceive, int countToReceive)
		{
			if (countToGive <= 0 || countToReceive <= 0)
				return Result<PlayerTrade>.Failure("Count must be greater that 0");
			return Result<PlayerTrade>.Success(new PlayerTrade(playerToGive, resourceToGive, countToGive, playerToReceive, resourceToReceive, countToReceive));
		}

		public void SetAccepted()
		{
			Status = TradeStatus.Accepted;
		}

	}
}
