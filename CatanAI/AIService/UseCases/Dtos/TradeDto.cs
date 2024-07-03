using AIService.Entities.Data;

namespace AIService.UseCases.Dtos
{
	public class PlayerTradeRequest
	{
		public Guid PlayerToGiveId { get; set; }
		public string ResourceToGive { get; set; }
		public int CountToGive { get; set; }
		public Guid PlayerToReceiveId { get; set; }
		public string ResourceToReceive { get; set; }
		public int CountToReceive { get; set; }
	}
}
