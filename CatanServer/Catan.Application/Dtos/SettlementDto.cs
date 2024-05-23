namespace Catan.Application.Dtos
{
	public class SettlementDto
	{
		public Guid PlayerId { get; set; }
		public int Position { get; set; }
		public bool IsCity { get; set; }
	}
}
