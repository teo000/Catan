namespace Catan.Domain.Entities
{
	public class Road
	{
		public Road(Player player, int position)
		{
			Player = player;
			Position = position;
		}
		public int Position { get; }
		public Player Player { get; }

	}
}
