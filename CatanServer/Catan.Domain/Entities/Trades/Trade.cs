using Catan.Domain.Common;
using Catan.Domain.Data;

namespace Catan.Domain.Entities.Trades
{
    public class Trade
    {
        protected Trade(Player playerToGive, Resource resourceToGive, int countToGive, Resource resourceToReceive, int countToReceive)
        {
            Id = Guid.NewGuid();
            PlayerToGive = playerToGive;
            ResourceToGive = resourceToGive;
            CountToGive = countToGive;
            ResourceToReceive = resourceToReceive;
            CountToReceive = countToReceive;
        }

        public Guid Id { get; }
		public Player PlayerToGive { get; }
		public Resource ResourceToGive { get; }
		public int CountToGive { get; }
        public Resource ResourceToReceive { get; }
        public int CountToReceive { get; }
    }
}
