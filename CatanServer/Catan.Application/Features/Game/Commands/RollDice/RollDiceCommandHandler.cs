using MediatR;
using Catan.Application.Responses;
using AutoMapper;
using Catan.Domain.Entities;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;

namespace Catan.Application.Features.Game.Commands.RollDice
{
	public class RollDiceCommandHandler : IRequestHandler<RollDiceCommand, DiceRollResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public RollDiceCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<DiceRollResponse> Handle(RollDiceCommand request, CancellationToken cancellationToken)
		{
			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new DiceRollResponse()
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
				return new DiceRollResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player does not exist." }
				};
			}

			if (!player.IsActive)
			{
				return new DiceRollResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player has been disconnected." }
				};
			}

			var result = gameSession.RollDice(player);
			if (!result.IsSuccess)
			{
				return new DiceRollResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
				};
			}

			return new DiceRollResponse()
			{
				Success = true,
				DiceRoll = _mapper.Map<DiceRollDto>(gameSession.Dice)
			};
		}
	}
}
