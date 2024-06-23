
namespace AIService.Entities.Moves
{
	public class PlaceRoadMove : Move
	{
		public PlaceRoadMove(Guid gameId, int position) : base(gameId)
		{
			Position = position;
			MoveType = Data.MoveType.PlaceRoad;
		}

		public int Position { get; set; }
	}
}
