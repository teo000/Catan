using MediatR;
using Catan.Application.Responses;
using Catan.Application.GameManagement;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using AutoMapper;
using Catan.Application.Dtos.GamePieces;
using Catan.Application.Dtos;
using Catan.Domain.Common;

namespace Catan.Application.Features.Game.Commands.MakeMove
{
	public class MakeMoveCommandHandler : IRequestHandler<MakeMoveCommand, GameSessionResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public MakeMoveCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<GameSessionResponse> Handle(MakeMoveCommand request, CancellationToken cancellationToken)
		{
			var validator = new MakeMoveCommandValidator();
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
				return  new GameSessionResponse()
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

			var moveType = (MoveType)Enum.Parse(typeof(MoveType), request.MoveType, true);

			return moveType switch
			{
				MoveType.PlaceRoad => HandlePlaceRoad(gameSession, player, request.Position),
				MoveType.PlaceSettlement => HandlePlaceSettlement(gameSession, player, request.Position),
				MoveType.PlaceCity => HandlePlaceCity(gameSession, player, request.Position),
				MoveType.BuyDevelopmentCard => HandleBuyDevelopmentCard(gameSession, player),
				_ => new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = ["Move is not available."]
				}
			};
		}

		private GameSessionResponse HandleBuyDevelopmentCard(GameSession gameSession, Player player)
		{
			var result = _gameSessionManager.BuyDevelopmentCard(gameSession, player);
			if (!result.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = [result.Error]
				};
			}
			return new GameSessionResponse() {
				Success = true,
				GameSession = _mapper.Map<GameSessionDto>(gameSession)
			};
		}

		private GameSessionResponse HandlePlaceRoad(GameSession gameSession, Player player, int? position)
		{
			var roadResult = _gameSessionManager.PlaceRoad(gameSession, player, position);
			if (!roadResult.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = [roadResult.Error]
				};
			}
			return new GameSessionResponse()
			{
				Success = true,
				GameSession = _mapper.Map<GameSessionDto>(gameSession)
			};
		}

		private GameSessionResponse HandlePlaceSettlement(GameSession gameSession, Player player, int? position)
		{
			var settlementResult = _gameSessionManager.PlaceSettlement(gameSession, player, position);
			if (!settlementResult.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = [settlementResult.Error]
				};
			}
			return new GameSessionResponse()
			{
				Success = true,
				GameSession = _mapper.Map<GameSessionDto>(gameSession)
			};
		}

		private GameSessionResponse HandlePlaceCity(GameSession gameSession, Player player, int? position)
		{
			var cityResult = _gameSessionManager.PlaceCity(gameSession, player, position);
			if (!cityResult.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = [cityResult.Error]
				};
			}
			return new GameSessionResponse()
			{
				Success = true,
				GameSession = _mapper.Map<GameSessionDto>(gameSession)
			};
		}

		//private MoveResponse HandleMoveThief(GameSession gameSession, Player player, int? position)
		//{
		//	var thiefResult = _gameSessionManager.PlaceCity(gameSession, player, position);
		//	if (!thiefResult.IsSuccess)
		//	{
		//		return new MoveResponse([thiefResult.Error]);
		//	}
		//	return new MoveResponse(_mapper.Map<CityDto>(thiefResult.Value));
		//}

	}
}
