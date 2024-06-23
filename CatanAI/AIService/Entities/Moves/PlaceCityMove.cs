namespace AIService.Entities.Moves
{
	public class PlaceCityMove : Move
	{
		public PlaceCityMove(Guid gameId, int position) : base(gameId)
		{
			Position = position;
			MoveType = Data.MoveType.PlaceRoad;
		}

		public int Position { get; set; }
	}
}
