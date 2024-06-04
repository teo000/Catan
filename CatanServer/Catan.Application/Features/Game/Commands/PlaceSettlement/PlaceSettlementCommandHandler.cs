using MediatR;
using Catan.Application.Dtos;
using Catan.Domain.Entities;
using Catan.Application.Responses;
using Catan.Application.Dtos.GamePieces;

namespace Catan.Application.Features.Game.Commands.PlaceSettlement
{
    public class PlaceSettlementCommandHandler : IRequestHandler<PlaceSettlementCommand, SettlementResponse>
	{
		private readonly GameSessionManager _gameSessionManager;

		public PlaceSettlementCommandHandler(GameSessionManager gameSessionManager)
		{
			_gameSessionManager = gameSessionManager;
		}

		public async Task<SettlementResponse> Handle(PlaceSettlementCommand request, CancellationToken cancellationToken)
		{
			var validator = new PlaceSettlementCommandValidator();
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid)
				return new SettlementResponse
				{
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};

			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new SettlementResponse()
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
				return new SettlementResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player does not exist." }
				};
			}

			if (!player.IsActive)
			{
				return new SettlementResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player has been disconnected." }
				};
			}

			var result = gameSession.PlaceSettlement(player, request.Position);

			if (!result.IsSuccess)
			{
				return new SettlementResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
				};
			}

			return new SettlementResponse()
			{
				Success = true,
				Settlement = new SettlementDto()
				{
					PlayerId = player.Id,
					Position = result.Value.Position,
				}
			};

		
		}
	}
}
