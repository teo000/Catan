using Catan.Application.Features.Game.Commands.EndTurn;
using Catan.Application.Features.Game.Commands.MakeMove;
using Catan.Application.Features.Game.Commands.MoveThief;
using Catan.Application.Features.Game.Commands.RollDice;
using Catan.Application.Features.Game.CommandsObsolete.CreateGame;
using Catan.Application.Features.Game.CommandsObsolete.PlaceCity;
using Catan.Application.Features.Game.CommandsObsolete.PlaceRoad;
using Catan.Application.Features.Game.CommandsObsolete.PlaceSettlement;
using Catan.Application.Features.Game.Queries.GetGameState;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using Catan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers
{
    public class GameController : ApiControllerBase
	{

		private readonly ILogger<GameController> _logger;
		private readonly GameSessionManager _gameSessionManager;

		public GameController(ILogger<GameController> logger, GameSessionManager gameSessionManager)
		{
			_logger = logger;
			_gameSessionManager = gameSessionManager;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id, [FromQuery] Guid playerId)
		{
			var result = await Mediator.Send(new GetGameState(id, playerId));
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("thief")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> MoveThief(MoveThiefCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("end-turn")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> EndTurn(EndTurnCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("roll-dice")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RollDice(RollDiceCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("notify-ai")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> NotifyAI(Guid gameId, Guid aIPlayerId)
		{
			var gameSessionResult = _gameSessionManager.GetGameSession(gameId);

			if (!gameSessionResult.IsSuccess) return BadRequest();
			var gameSession = gameSessionResult.Value;

			Player aIPlayer = null;

			foreach (var p in gameSession.Players)
			{
				if (p.Id == aIPlayerId)
				{
					aIPlayer = p; break;
				}
			}
			if (aIPlayer == null) return BadRequest();

			await _gameSessionManager.HandleAIPlayer(gameSession, aIPlayer);
			return Ok();
		}

		[HttpPost("make-move")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> MakeMove(MakeMoveCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(CreateGameCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("road")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PlaceRoad(PlaceRoadCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("settlement")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PlaceSettlement(PlaceSettlementCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("city")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PlaceCity(PlaceCityCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

	}
}
