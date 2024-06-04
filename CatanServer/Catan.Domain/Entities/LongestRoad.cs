using Catan.Domain.Entities.GamePieces;

namespace Catan.Domain.Entities
{
    public class LongestRoad
	{
		public LongestRoad(List<Road> roads, Player player) {
			Roads = new List<Road>(roads);
			Player = player;
		}
		public List<Road> Roads { get;}
		public Player Player { get; }
	}
}
