using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Responses;
using Catan.Domain.Data;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.AcceptTrade
{
	public class AcceptTradeCommandHandler : IRequestHandler<AcceptTradeCommand, TradeResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;

		public AcceptTradeCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
		}

		public async Task<TradeResponse> Handle(AcceptTradeCommand request, CancellationToken cancellationToken)
		{
			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new TradeResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { gameSessionResponse.Error }
				};
			}

			var gameSession = gameSessionResponse.Value;
			var tradeResponse = gameSession.GetTrade(request.TradeId);

			if (!tradeResponse.IsSuccess) {
				return new TradeResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { tradeResponse.Error }
				};
			}

			var trade = tradeResponse.Value;
			if (trade.Status != TradeStatus.Pending)
				return new TradeResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Trade is no longer available." }
				};

			if (trade.PlayerToReceive.Id != request.PlayerId)
				return new TradeResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "You cannot respond to this trade" }
				};

			var result = gameSession.AcceptTrade(trade.Id);
			if (!result.IsSuccess)
			{
				return new TradeResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() {  result.Error}
				};
			}

			return new TradeResponse()
			{
				Success = true,
				Trade = _mapper.Map<TradeDto>(result.Value)
			};

		}
	}
}
