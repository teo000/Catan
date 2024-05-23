namespace Catan.Application.Dtos
{
	public class TradeDto
	{
		public Guid Id { get; set; }
		public Guid PlayerToGiveId { get; set; }
		public string ResourceToGive { get; set; }
		public int CountToGive { get; set; }
		public Guid PlayerToReceiveId { get; set; }
		public string ResourceToReceive { get; set; }
		public int CountToReceive { get; set; }
		public string Status { get; private set; }
	}
}