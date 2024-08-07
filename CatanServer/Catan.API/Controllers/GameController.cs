using Catan.API.Hubs;
using Catan.Application.Features.Game.Commands.DiscardHalf;
using Catan.Application.Features.Game.Commands.EndTurn;
using Catan.Application.Features.Game.Commands.MakeMove;
using Catan.Application.Features.Game.Commands.MoveThief;
using Catan.Application.Features.Game.Commands.PlayDevelopmentCard;
using Catan.Application.Features.Game.Commands.RollDice;
using Catan.Application.Features.Game.Queries.GetGameState;
using Catan.Application.GameManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Catan.API.Controllers
{
	public class GameController : ApiControllerBase
	{
		private readonly IHubContext<GameHub> _hubContext;
		private readonly Domain.Interfaces.ILogger _logger;

		public GameController(IHubContext<GameHub> hubContext, Domain.Interfaces.ILogger logger)
		{
			_hubContext = hubContext;
			_logger = logger;
		}

		[Authorize(Roles = "User")]
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id, [FromQuery] Guid playerId)
		{
			var result = await Mediator.Send(new GetGameState(id, playerId));
			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}

		[Authorize(Roles = "User")]
		[HttpPost("thief")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> MoveThief(MoveThiefCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			//_logger.Warn($"Make move: game session sent this to all clients: {result.GameSession}");
			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);

			return Ok(result);
		}

		[Authorize(Roles = "User")]
		[HttpPost("end-turn")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> EndTurn(EndTurnCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			//_logger.Warn($"Make move: game session sent this to all clients: {result.GameSession}");
			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);

			return Ok(result);
		}

		[Authorize(Roles = "User")]
		[HttpPost("roll-dice")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RollDice(RollDiceCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			//_logger.Warn($"Make move: game session sent this to all clients: {result.GameSession}");
			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);

			return Ok(result);
		}

		[Authorize]
		[HttpPost("make-move")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> MakeMove(MakeMoveCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);

			return Ok(result);
		}


		[Authorize(Roles = "User")]
		[HttpPost("play-devcard")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PlayDevelopmentCard(PlayDevelopmentCardCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);

			return Ok(result);
		}

		[Authorize(Roles = "User")]
		[HttpPost("discard-half")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DiscardHalf(DiscardHalfCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			//_logger.Warn($"Make move: game session sent this to all clients: {result.GameSession}");
			await _hubContext.Clients.Group(command.GameId.ToString()).SendAsync("ReceiveGame", result.GameSession);

			return Ok(result);
		}


		//// for testing purposes
		//[HttpPost("notify-ai")]
		//[ProducesResponseType(StatusCodes.Status201Created)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> NotifyAI(Guid gameId, Guid aIPlayerId)
		//{
		//	var gameSessionResult = _gameSessionManager.GetGameSession(gameId);

		//	if (!gameSessionResult.IsSuccess) return BadRequest();
		//	var gameSession = gameSessionResult.Value;

		//	Player aIPlayer = null;

		//	foreach (var p in gameSession.Players)
		//	{
		//		if (p.Id == aIPlayerId)
		//		{
		//			aIPlayer = p; break;
		//		}
		//	}
		//	if (aIPlayer == null) return BadRequest();

		//	await _gameSessionManager.HandleAIPlayer(gameSession, aIPlayer);
		//	return Ok();
		//}


		//// obsolete, for testing purposes
		//[HttpPost]
		//[ProducesResponseType(StatusCodes.Status201Created)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> Create(CreateGameCommand command)
		//{
		//	var result = await Mediator.Send(command);
		//	if (!result.Success)
		//		return BadRequest(result);
		//	return Ok(result);
		//}

		//// obsolete, for testing purposes
		//[HttpPost("road")]
		//[ProducesResponseType(StatusCodes.Status201Created)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> PlaceRoad(PlaceRoadCommand command)
		//{
		//	var result = await Mediator.Send(command);
		//	if (!result.Success)
		//		return BadRequest(result);
		//	return Ok(result);
		//}

		//// obsolete, for testing purposes
		//[HttpPost("settlement")]
		//[ProducesResponseType(StatusCodes.Status201Created)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> PlaceSettlement(PlaceSettlementCommand command)
		//{
		//	var result = await Mediator.Send(command);
		//	if (!result.Success)
		//		return BadRequest(result);
		//	return Ok(result);
		//}

		//// obsolete, for testing purposes
		//[HttpPost("city")]
		//[ProducesResponseType(StatusCodes.Status201Created)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> PlaceCity(PlaceCityCommand command)
		//{
		//	var result = await Mediator.Send(command);
		//	if (!result.Success)
		//		return BadRequest(result);
		//	return Ok(result);
		//}

	}
}
