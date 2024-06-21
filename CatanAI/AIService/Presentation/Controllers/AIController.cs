using AIService.Entities.Data;
using AIService.Presentation.Requests;
using AIService.UseCases;
using AIService.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
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

			return Ok(discardResult);

			//if (discardResult.IsSuccess)
			//{
			//	var jsonOptions = new JsonSerializerOptions
			//	{
			//		WriteIndented = true,
			//		Converters =
			//		{
			//			new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
			//		}
			//	};

			//	var responseContent = JsonSerializer.Serialize(discardResult, jsonOptions);
			//	return new JsonResult(responseContent)
			//	{
			//		StatusCode = 200,
			//		ContentType = "application/json"
			//	};
			//}

			return BadRequest(discardResult.Error);
		}
	}
}
