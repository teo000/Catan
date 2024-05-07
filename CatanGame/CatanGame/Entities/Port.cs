using CatanGame.Data;

namespace CatanGame.Entities
{
	public class Port : Tile
	{
		public Resources ResourceToGive { get; private set; }
		public int NoToGive { get; private set; }
		public Resources ResourceToTake { get; private set; }
		public int NoToTake { get; private set; }

	}
}
