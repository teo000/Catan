namespace AIService.Entities.Game.GamePieces
{
	public abstract class GamePiece
	{
		public int Position { get; set; }
		public Guid PlayerId { get; set; }

		public bool BelongsTo(Guid playerId)
		{
			if (Equals(playerId, PlayerId))
				return true;
			return false;
		}
	}
}