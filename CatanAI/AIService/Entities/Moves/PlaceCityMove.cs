namespace AIService.Entities.Moves
{
	public class PlaceCityMove : Move
	{
		public PlaceCityMove(Guid gameId, int position) : base(gameId)
		{
			Position = position;
			MoveType = Data.MoveType.PlaceCity;
		}

		public int Position { get; set; }
	}
}
