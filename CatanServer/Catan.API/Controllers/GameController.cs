using Catan.Application.Features.Game.Commands.CreateGame;
using Catan.Application.Features.Game.Queries.GetGameState;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers
{
    public class GameController : ApiControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

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
	}
}
