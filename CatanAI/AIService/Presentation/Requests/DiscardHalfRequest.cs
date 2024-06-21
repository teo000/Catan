using AIService.Entities.Game;

namespace AIService.Presentation.Requests
{
	public class DiscardHalfRequest
	{
		public GameState GameState {  get; set; }
		public Guid PlayerId { get; set; }
		public Dictionary<string, int> ResourceCount { get; set; }
	}
}
