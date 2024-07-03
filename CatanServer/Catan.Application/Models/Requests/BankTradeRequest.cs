namespace Catan.Application.Models.Requests
{
	public class BankTradeRequest
	{
		public Guid PlayerToGiveId { get; set; }
		public string ResourceToGive { get; set; }
		public int CountToGive { get; set; }
		public string ResourceToReceive { get; set; }
		public int CountToReceive { get; set; }
	}
}
