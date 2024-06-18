using AIService.Presentation.Requests;
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
		public async Task<IActionResult> TriggerAIMove([FromBody] MoveRequest request)
		{
			var moveResult = _aiDecisionService.DecideNextMove(request.GameState, request.PlayerId);

			if(moveResult.IsSuccess)
				return Ok(moveResult);
			return BadRequest(moveResult.Error);
		}
	}
}
