using AIService.Entities.Game;
using AIService.Interfaces;
using AIService.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace AIService.Presentation.Controllers
{
    [ApiController]
	[Route("api/ai")]
	public class AIController : ControllerBase
	{
		private readonly IGameBackendService _gameBackendService;
		private readonly AIDecisionService _aiDecisionService;

		public AIController(IGameBackendService gameBackendService, AIDecisionService aiDecisionService)
		{
			_gameBackendService = gameBackendService;
			_aiDecisionService = aiDecisionService;
		}

		[HttpPost("trigger-aimove")]
		public async Task<IActionResult> TriggerAIMove([FromBody] GameState gameState)
		{
			var move = _aiDecisionService.DecideNextMove(gameState);
			var result = await _gameBackendService.NotifyMoveAsync(move);

			if(result.IsSuccess)
				return Ok(move);
			return BadRequest(result.Error);
		}
	}
}
