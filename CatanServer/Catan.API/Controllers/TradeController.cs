using Catan.Application.Features.Trade.Commands.AcceptTrade;
using Catan.Application.Features.Trade.Commands.InitiateTrade;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers
{
    public class TradeController : ApiControllerBase
	{
		private readonly ILogger<GameController> _logger;

		public TradeController(ILogger<GameController> logger)
		{
			_logger = logger;
		}

		[HttpPost("initiate")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Initiate(InitiateTradeCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("accept")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Accept(AcceptTradeCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

	}
}
