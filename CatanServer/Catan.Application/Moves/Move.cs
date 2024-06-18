namespace Catan.Application.Moves
{
	public class Move
	{
		public Guid GameId { get; set; }
		public string MoveType { get; set; }
		public int? Position { get; set; }
	}
}
