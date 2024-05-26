using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Responses;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Game.Commands.EndTurn
{
	public class EndTurnCommandHandler : IRequestHandler<EndTurnCommand, GameSessionResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public EndTurnCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<GameSessionResponse> Handle(EndTurnCommand request, CancellationToken cancellationToken)
		{
			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { gameSessionResponse.Error }
				};
			}

			var gameSession = gameSessionResponse.Value;
			bool playerExists = false;
			Player player = null;

			foreach (var p in gameSession.Players)
			{
				if (p.Id == request.PlayerId)
				{
					playerExists = true;
					player = p;
					break;
				}
			}

			if (!playerExists)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player does not exist." }
				};
			}

			if (!player.IsActive)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player has been disconnected." }
				};
			}

			if (player != gameSession.GetTurnPlayer())
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "It is not your turn." }
				};
			}

			_gameSessionManager.EndPlayerTurn(gameSession);


			return new GameSessionResponse()
			{
				Success = true,
				GameSession = _mapper.Map<GameSessionDto>(gameSession)
			};
		}
	}
}
