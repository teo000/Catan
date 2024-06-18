using AIService.Entities.Game;
using AIService.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace AIService.Presentation.Controllers
{
    [ApiController]
	[Route("api/ai")]
	public class AIController : ControllerBase
	{
		private readonly AIDecisionService _aiDecisionService;

		public AIController(AIDecisionService aiDecisionService)
		{
			_aiDecisionService = aiDecisionService;
		}

		[HttpPost("trigger-aimove")]
		public async Task<IActionResult> TriggerAIMove([FromBody] GameState gameState)
		{
			var moveResult = _aiDecisionService.DecideNextMove(gameState);

			if(moveResult.IsSuccess)
				return Ok(moveResult);
			return BadRequest(moveResult.Error);
		}
	}
}
