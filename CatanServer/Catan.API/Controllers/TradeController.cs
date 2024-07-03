using Catan.API.Hubs;
using Catan.Application.Features.Trade.Commands.InitiateTrade;
using Catan.Application.Features.Trade.Commands.RespondToTrade;
using Catan.Application.Features.Trade.Commands.TradeBank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Catan.API.Controllers
{
	public class TradeController : ApiControllerBase
	{
		private readonly Domain.Interfaces.ILogger _logger;
		private readonly IHubContext<GameHub> _hubContext;

		public TradeController(IHubContext<GameHub> hubContext, Domain.Interfaces.ILogger logger)
		{
			_logger = logger;
			_hubContext = hubContext;
		}

		[Authorize(Roles = "User")]
		[HttpPost("initiate")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Initiate(InitiateTradeCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			_logger.Warn($"Make move: game session sent this to all clients: {result.GameSession}");
			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);


			return Ok(result);
		}

		[Authorize(Roles = "User")]
		[HttpPost("accept")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Accept(RespondToTradeCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			_logger.Warn($"Make move: game session sent this to all clients: {result.GameSession}");
			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);


			return Ok(result);
		}

		[Authorize(Roles = "User")]
		[HttpPost("bank")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> TradeBank(TradeBankCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			_logger.Warn($"Make move: game session sent this to all clients: {result.GameSession}");
			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);

			return Ok(result);
		}
	}
}
