using Catan.Domain.Data;

namespace Catan.Domain.Entities
{
	public class Port : Tile
	{
		public Resource ResourceToGive { get; private set; }
		public int NoToGive { get; private set; }
		public Resource ResourceToTake { get; private set; }
		public int NoToTake { get; private set; }

	}
}
