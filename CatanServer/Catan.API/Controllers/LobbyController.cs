using Catan.Application.Features.Game.Queries.GetGameState;
using Catan.Application.Features.Lobby.CreateLobby;
using Catan.Application.Features.Lobby.GetLobby;
using Catan.Application.Features.Lobby.JoinLobby;
using Catan.Application.Features.Lobby.StartGame;
using Catan.Application.Features.Trade.Commands.AcceptTrade;
using Catan.Application.Features.Trade.Commands.InitiateTrade;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers
{
	public class LobbyController : ApiControllerBase
	{
		private readonly ILogger<GameController> _logger;

		public LobbyController(ILogger<GameController> logger)
		{
			_logger = logger;
		}


		[HttpGet("{joinCode}")]
		public async Task<IActionResult> Get(string joinCode)
		{
			var result = await Mediator.Send(new GetLobbyQuery(joinCode));
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("create")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create(CreateLobbyCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("join")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Join(JoinLobbyCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("start")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Start(StartGameCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);
			return Ok(result);
		}

	}
}
