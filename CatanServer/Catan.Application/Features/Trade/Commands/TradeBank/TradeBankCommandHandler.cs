using Catan.Application.Features.Trade.Commands.InitiateTrade;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Trade.Commands.TradeBank
{
	public class TradeBankCommandHandler : IRequestHandler<TradeBankCommand, BaseResponse>
	{
		private GameSessionManager _gameSessionManager;

		public TradeBankCommandHandler(GameSessionManager gameSessionManager)
		{
			_gameSessionManager = gameSessionManager;
		}

		public async Task<BaseResponse> Handle(TradeBankCommand request, CancellationToken cancellationToken)
		{
			var validator = new TradeBankCommandValidator();
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid)
				return new BaseResponse
				{
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};

			var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

			if (!gameSessionResponse.IsSuccess)
			{
				return new BaseResponse()
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
				return new BaseResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player does not exist." }
				};
			}

			if (!player.IsActive)
			{
				return new BaseResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { "Player has been disconnected." }
				};
			}

			var resourceToGive = (Resource)Enum.Parse(typeof(Resource), request.ResourceToGive, true);
			var resourceToReceive = (Resource)Enum.Parse(typeof(Resource), request.ResourceToReceive, true);

			var result = gameSession.TradeBank(player, resourceToGive, request.Count,  resourceToReceive);
			if (!result.IsSuccess) {
				return new BaseResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
				};
			}
			return new BaseResponse()
			{
				Success = true,
			};
		}
	}
}
