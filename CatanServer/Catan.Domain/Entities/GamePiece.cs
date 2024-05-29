namespace Catan.Domain.Entities
{
	public abstract class GamePiece
	{
		protected GamePiece(Player player, int position)
		{
			Position = position;
			Player = player;
		}

		public int Position { get; }
		public Player Player { get; }

		public bool BelongsTo(Player player)
		{
			if (player == Player)
				return true;
			return false;
		}
	}
}
