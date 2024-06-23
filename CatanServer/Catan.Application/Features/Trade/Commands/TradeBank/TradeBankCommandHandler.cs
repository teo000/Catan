using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Features.Trade.Commands.InitiateTrade;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.TradeBank
{
	public class TradeBankCommandHandler : IRequestHandler<TradeBankCommand, GameSessionResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public TradeBankCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<GameSessionResponse> Handle(TradeBankCommand request, CancellationToken cancellationToken)
		{
			var validator = new TradeBankCommandValidator();
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid)
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};

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

			var resourceToGive = (Resource)Enum.Parse(typeof(Resource), request.ResourceToGive, true);
			var resourceToReceive = (Resource)Enum.Parse(typeof(Resource), request.ResourceToReceive, true);

			var result = gameSession.TradeBank(player, resourceToGive, request.Count,  resourceToReceive);
			if (!result.IsSuccess) {
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
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
