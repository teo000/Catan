namespace CatanGame.Entities
{
	public class Settlement
	{
		public Settlement(Player player, bool isCity)
		{
			Player = player;
			IsCity = isCity;
		}
		public int position { get; set; }
		public bool IsCity { get; private set; }
		public Player Player { get; private set; }

		public bool BelongsTo (Player player)
		{
			if (player == Player)
				return true;
			return false;
		}
	}
}
