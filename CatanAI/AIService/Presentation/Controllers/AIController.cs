using AIService.Presentation.Requests;
using AIService.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

		[HttpPost("discard-half")]
		public async Task<IActionResult> DiscardHalf([FromBody] MoveRequest request)
		{
			var discardResult = _aiDecisionService.DiscardHalf(request.GameState, request.PlayerId);
			if (discardResult.IsSuccess)
				return Ok(discardResult);

			return BadRequest(discardResult.Error);
		}

		[HttpPost("move-thief")]
		public async Task<IActionResult> MoveThief([FromBody] MoveRequest request)
		{
			var discardResult = _aiDecisionService.MoveThief(request.GameState, request.PlayerId);
			if (discardResult.IsSuccess)
				return Ok(discardResult);

			return BadRequest(discardResult.Error);
		}

		[HttpPost("respond-to-trade")]
		public async Task<IActionResult> RespondToTrade([FromBody] TradeRequest request)
		{
			var result = _aiDecisionService.RespondToTrade(request.GameState, request.PlayerId, request.Trade);
			if (result.IsSuccess)
				return Ok(result);

			return BadRequest(result.Error);
		}

		[HttpPost("initiate-player-trades")]
		public async Task<IActionResult> InitiatePlayerTrades([FromBody] MoveRequest request)
		{
			var result = _aiDecisionService.InitiatePlayerTrades(request.GameState, request.PlayerId);

			var responseJson = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });


			return Ok(result);

		}
	}
}
