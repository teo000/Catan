namespace AIService.Entities.Moves
{
	public class PlaceSettlementMove : Move
	{
		public PlaceSettlementMove(Guid gameId, int position) : base(gameId)
		{ 
			Position = position;
			MoveType = Data.MoveType.PlaceSettlement;
		}
		public int Position { get; set; }
	}
}
