using MediatR;
using Catan.Application.Responses;
using AutoMapper;
using Catan.Domain.Entities;
using Catan.Application.Dtos.GamePieces;
using Catan.Application.GameManagement;

namespace Catan.Application.Features.Game.Commands.PlaceCity
{
	public class PlaceCityCommandHandler : IRequestHandler<PlaceCityCommand, CityResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public PlaceCityCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<CityResponse> Handle(PlaceCityCommand request, CancellationToken cancellationToken)
		{
			var validator = new PlaceCityCommandValidator();
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid)
				return new CityResponse	
				{
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};

			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new CityResponse()
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
				return new CityResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player does not exist." }
				};
			}

			if (!player.IsActive)
			{
				return new CityResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player has been disconnected." }
				};
			}

			var result = gameSession.PlaceCity(player, request.Position);

			if (!result.IsSuccess)
			{
				return new CityResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
				};
			}

			return new CityResponse()
			{
				Success = true,
				City = new CityDto()
				{
					PlayerId = player.Id,
					Position = result.Value.Position,
				}
			};


		}
	}
}
