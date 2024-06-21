using MediatR;
using Catan.Application.Responses;
using Catan.Application.GameManagement;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using AutoMapper;
using Catan.Application.Dtos.GamePieces;
using Catan.Application.Dtos;

namespace Catan.Application.Features.Game.Commands.MakeMove
{
	public class MakeMoveCommandHandler : IRequestHandler<MakeMoveCommand, MoveResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public MakeMoveCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<MoveResponse> Handle(MakeMoveCommand request, CancellationToken cancellationToken)
		{
			var validator = new MakeMoveCommandValidator();
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid)
				return new MoveResponse(validatorResult.Errors.Select(e => e.ErrorMessage).ToList());


			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new MoveResponse([gameSessionResponse.Error]);
			}

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
				return new MoveResponse(["Player does not exist."]);
			}

			if (!player.IsActive)
			{
				return new MoveResponse(["Player has been disconnected."]);
			}

			var moveType = (MoveType)Enum.Parse(typeof(MoveType), request.MoveType, true);

			return moveType switch
			{
				MoveType.PlaceRoad => HandlePlaceRoad(gameSession, player, request.Position),
				MoveType.PlaceSettlement => HandlePlaceSettlement(gameSession, player, request.Position),
				MoveType.PlaceCity => HandlePlaceCity(gameSession, player, request.Position),
				MoveType.BuyDevelopmentCard => HandleBuyDevelopmentCard(gameSession, player),
				_ => new MoveResponse(["Move is not available"]),
			};
		}

		private MoveResponse HandleBuyDevelopmentCard(GameSession gameSession, Player player)
		{
			var result = _gameSessionManager.BuyDevelopmentCard(gameSession, player);
			if (!result.IsSuccess)
			{
				return new MoveResponse([result.Error]);
			}
			return new MoveResponse(_mapper.Map<DevelopmentCardDto>(result.Value));
		}

		private MoveResponse HandlePlaceRoad(GameSession gameSession, Player player, int? position)
		{
			var roadResult = _gameSessionManager.PlaceRoad(gameSession, player, position);
			if (!roadResult.IsSuccess)
			{
				return new MoveResponse([roadResult.Error]);
			}
			return new MoveResponse(_mapper.Map<RoadDto>(roadResult.Value));
		}

		private MoveResponse HandlePlaceSettlement(GameSession gameSession, Player player, int? position)
		{
			var settlementResult = _gameSessionManager.PlaceSettlement(gameSession, player, position);
			if (!settlementResult.IsSuccess)
			{
				return new MoveResponse([settlementResult.Error]);
			}
			return new MoveResponse(_mapper.Map<SettlementDto>(settlementResult.Value));
		}

		private MoveResponse HandlePlaceCity(GameSession gameSession, Player player, int? position)
		{
			var cityResult = _gameSessionManager.PlaceCity(gameSession, player, position);
			if (!cityResult.IsSuccess)
			{
				return new MoveResponse([cityResult.Error]);
			}
			return new MoveResponse(_mapper.Map<CityDto>(cityResult.Value));
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
