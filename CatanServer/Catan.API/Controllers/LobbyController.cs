using Catan.API.Hubs;
using Catan.Application.Features.Lobby.AddAIPlayer;
using Catan.Application.Features.Lobby.CreateLobby;
using Catan.Application.Features.Lobby.GetLobby;
using Catan.Application.Features.Lobby.JoinLobby;
using Catan.Application.Features.Lobby.StartGame;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Catan.API.Controllers
{
	public class LobbyController : ApiControllerBase
	{
		private readonly IHubContext<LobbyHub> _hubContext;
		private readonly ILogger<GameController> _logger;

		public LobbyController(IHubContext<LobbyHub> hubContext , ILogger<GameController> logger)
		{
			_hubContext = hubContext;
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

		[Authorize(Roles = "User")]
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

			await _hubContext.Clients.Group(command.JoinCode.ToString()).SendAsync("ReceiveLobby", result.Lobby);

			return Ok(result);
		}

		[HttpPost("add-ai")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddAI(AddAIPlayerCommand command)
		{
			var result = await Mediator.Send(command);
			if (!result.Success)
				return BadRequest(result);

			await _hubContext.Clients.Group(command.JoinCode.ToString()).SendAsync("ReceiveLobby", result.Lobby);

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

			await _hubContext.Clients.Group(command.JoinCode.ToString()).SendAsync("ReceiveLobby", result.Lobby);

			return Ok(result);
		}

	}
}
