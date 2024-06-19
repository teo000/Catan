namespace Catan.Application.Moves
{
	public class Move
	{
		public Guid GameId { get; set; }
		public string MoveType { get; set; }
		public int? Position { get; set; }

		public override string ToString()
		{
			return $"Move [GameId={GameId}, MoveType={MoveType}, Position={Position}]";
		}

	}
}
