namespace Catan.Application.Dtos.GamePieces
{
    public class SettlementDto
    {
        public Guid PlayerId { get; set; }
        public int Position { get; set; }

		public override string ToString()
		{
			return $"SettlementDto {{ PlayerId = {PlayerId}, Position = {Position} }}";
		}

	}
}
