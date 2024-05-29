using Catan.Application.Features.Game.Commands.CreateGame;
using Catan.Application.Features.Game.Commands.EndTurn;
using Catan.Application.Features.Game.Commands.MoveThief;
using Catan.Application.Features.Game.Commands.PlaceCity;
using Catan.Application.Features.Game.Commands.PlaceRoad;
using Catan.Application.Features.Game.Commands.PlaceSettlement;
using Catan.Application.Features.Game.Commands.RollDice;
using Catan.Application.Features.Game.Queries.GetGameState;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers
{
    public class GameController : ApiControllerBase
	{

		private readonly ILogger<GameController> _logger;

		public GameController(ILogger<GameController> logger)
		{
			_logger = logger;
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
		

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var result = await Mediator.Send(new GetGameState(id));
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

	}
}
