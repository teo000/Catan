using AIService.Entities.Game;
using AIService.Entities.Moves;

namespace AIService.UseCases
{
    public class AIDecisionService
	{
		public Move DecideNextMove(GameState gameState)
		{
			return new EndTurnMove();
		}
	}
}
