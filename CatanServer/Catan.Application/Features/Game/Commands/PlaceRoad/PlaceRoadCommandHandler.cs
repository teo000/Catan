using Catan.Application.Dtos;
using Catan.Application.Responses;
using Catan.Domain.Common;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Game.Commands.PlaceRoad
{
    public class PlaceRoadCommandHandler : IRequestHandler<PlaceRoadCommand, RoadResponse>
	{
		private readonly GameSessionManager _gameSessionManager;

		public PlaceRoadCommandHandler(GameSessionManager gameSessionManager)
		{
			_gameSessionManager = gameSessionManager;
		}

		public async Task<RoadResponse> Handle(PlaceRoadCommand request, CancellationToken cancellationToken)
		{
			var validator = new PlaceRoadCommandValidator();
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid)
				return new RoadResponse
				{
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};

			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if ( !gameSessionResponse.IsSuccess )
			{
				return new RoadResponse()
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

			if ( !playerExists ) {
				return new RoadResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player does not exist." }
				};
			}

			if (! player.IsActive)
			{
				return new RoadResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player has been disconnected." }
				};
			}

			
			var result = gameSession.PlaceRoad(player, request.Position);

			if (!result.IsSuccess)
			{
				return new RoadResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
				};
			}

			return new RoadResponse()
			{
				Success = true,
				Road = new RoadDto()
				{
					PlayerId = player.Id,
					Position = result.Value.Position,
				}
			};

		}
	}
}
