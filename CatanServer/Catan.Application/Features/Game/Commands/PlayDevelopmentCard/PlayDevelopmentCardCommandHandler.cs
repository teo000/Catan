using MediatR;
using Catan.Application.Responses;
using Catan.Application.GameManagement;
using AutoMapper;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using Catan.Application.Dtos;
using System.Numerics;

namespace Catan.Application.Features.Game.Commands.PlayDevelopmentCard
{
	public class PlayDevelopmentCardCommandHandler : IRequestHandler<PlayDevelopmentCardCommand, GameSessionResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public PlayDevelopmentCardCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<GameSessionResponse> Handle(PlayDevelopmentCardCommand request, CancellationToken cancellationToken)
		{
			var validator = new PlayDevelopmentCardCommandValidator();
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid)
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};


			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = [gameSessionResponse.Error]
				};


			var gameSession = gameSessionResponse.Value;
			Player player = null;

			foreach (var p in gameSession.Players)
			{
				if (p.Id == request.PlayerId)
				{
					player = p; break;
				}
			}

			if (player == null)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = ["Player does not exist."]
				};
			}

			if (!player.IsActive)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = ["Player has been disconnected."]
				};
			}

			var moveType = (DevelopmentType)Enum.Parse(typeof(DevelopmentType), request.DevelopmentType, true);

			return moveType switch
			{
				DevelopmentType.KNIGHT => HandleKnight(gameSession, player, request.Position),
				_ => new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = ["Move is not available."]
				}
			} ;
		}

		private GameSessionResponse HandleKnight(GameSession gameSession, Player player, int? position)
		{
			if (!position.HasValue)
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = ["You must specify the position to move the knight to."]
				};

			var result = gameSession.HandleKnight(player, position.Value);
			if (!result.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = [result.Error]
				};
			}
			return new GameSessionResponse()
			{
				Success = true,
				GameSession = _mapper.Map<GameSessionDto>(gameSession)
			};
		}
	}
}
