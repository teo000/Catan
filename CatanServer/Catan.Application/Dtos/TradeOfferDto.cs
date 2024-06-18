namespace Catan.Application.Dtos
{
	public class TradeOfferDto
	{
		public Guid? PlayerToTrade { get; set; }
		public string ResourceToGive { get; set; }
		public string ResourceToReceive { get; set; }
		public int CountToGive { get; set; }
		public int? CountToReceive { get; set;}
	}
}
