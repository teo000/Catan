using AIService.Entities.Game;

namespace AIService.Presentation.Requests
{
	public class MoveRequest
	{
		public GameState GameState { get; set; }
		public Guid PlayerId { get; set; }
	}
}
