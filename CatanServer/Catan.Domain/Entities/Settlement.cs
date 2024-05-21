namespace Catan.Domain.Entities
{
	public class Settlement
	{
		public Settlement(Player player, bool isCity, int position)
		{
			Player = player;
			IsCity = isCity;
			Position = position;
		}
		public int Position { get; }
		public bool IsCity { get; }
		public Player Player { get; }

		public bool BelongsTo (Player player)
		{
			if (player == Player)
				return true;
			return false;
		}
	}
}
