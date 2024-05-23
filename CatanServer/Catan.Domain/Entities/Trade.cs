using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities
{
	public class Trade
	{
		private Trade(Player playerToGive, Resource resourceToGive, int countToGive, Player playerToReceive, Resource resourceToReceive, int countToReceive)
		{
			Id = Guid.NewGuid();
			PlayerToGive = playerToGive;
			ResourceToGive = resourceToGive;
			CountToGive = countToGive;
			PlayerToReceive = playerToReceive;
			ResourceToReceive = resourceToReceive;
			CountToReceive = countToReceive;
		}

		public Guid Id { get; }
		public Player PlayerToGive { get; }
		public Resource ResourceToGive { get; }
		public int CountToGive { get; }
		public Player PlayerToReceive { get; }
		public Resource ResourceToReceive { get; }
		public int CountToReceive { get; }
		public TradeStatus Status { get; private set; } = TradeStatus.Pending;

		public static Result<Trade> Create(Player playerToGive, Resource resourceToGive, int countToGive, Player playerToReceive, Resource resourceToReceive, int countToReceive)
		{
			if (countToGive <= 0 || countToReceive <= 0)
				return Result<Trade>.Failure("Count must be greater that 0");
			return Result<Trade>.Success(new Trade(playerToGive, resourceToGive, countToGive, playerToReceive, resourceToReceive, countToReceive));	
		}

		public void SetAccepted()
		{
			Status = TradeStatus.Accepted;
		}
	}
}
