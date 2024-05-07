namespace CatanGame.Entities
{
	public class Road
	{
		public Road(Player player)
		{
			Player = player;
		}
		public int position { get; set; }
		public Player Player { get; set; }

	}
}
