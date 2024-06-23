using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using Catan.Domain.Data;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.RespondToTrade
{
	public class RespondToTradeCommandHandler : IRequestHandler<RespondToTradeCommand, GameSessionResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public RespondToTradeCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<GameSessionResponse> Handle(RespondToTradeCommand request, CancellationToken cancellationToken)
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
			var tradeResponse = gameSession.GetTrade(request.TradeId);

			if (!tradeResponse.IsSuccess) {
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { tradeResponse.Error }
				};
			}

			var trade = tradeResponse.Value;
			if (trade.Status != TradeStatus.Pending)
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Trade is no longer available." }
				};

			if (trade.PlayerToReceive.Id != request.PlayerId)
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "You cannot respond to this trade" }
				};

			var result = gameSession.RespondToTrade(trade.Id, request.IsAccepted);
			if (!result.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() {  result.Error}
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
