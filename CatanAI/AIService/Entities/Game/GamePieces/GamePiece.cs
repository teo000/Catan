namespace AIService.Entities.Game.GamePieces
{
	public abstract class GamePiece
	{
		public int Position { get; set; }
		public Player Player { get; set; }

		public bool BelongsTo(Player player)
		{
			if (player == Player)
				return true;
			return false;
		}
	}
}