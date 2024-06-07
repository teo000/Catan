using MediatR;
using Catan.Application.Responses;
using AutoMapper;
using Catan.Domain.Entities;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;

namespace Catan.Application.Features.Game.Commands.MoveThief
{
    public class MoveThiefCommandHandler : IRequestHandler<MoveThiefCommand, MapResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public MoveThiefCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<MapResponse> Handle(MoveThiefCommand request, CancellationToken cancellationToken)
		{
			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new MapResponse()
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
				return new MapResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player does not exist." }
				};
			}

			if (!player.IsActive)
			{
				return new MapResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player has been disconnected." }
				};
			}

			var result = gameSession.MoveThief(player, request.Position);

			if (!result.IsSuccess)
			{
				return new MapResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
				};
			}

			return new MapResponse()
			{
				Success = true,
				Map = _mapper.Map<MapDto>(result.Value)
			};

		}
	}
}
