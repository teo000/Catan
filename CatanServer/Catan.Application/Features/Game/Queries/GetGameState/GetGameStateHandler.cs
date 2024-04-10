using MediatR;

namespace Catan.Application.Features.Game.Queries.GetGameState
{
	public class GetGameStateHandler : IRequestHandler<GetGameState, GameSessionResponse>
	{
		private IGameSessionManager _gameSessionManager;
		public GetGameStateHandler(IGameSessionManager gameSessionManager)
		{
			_gameSessionManager = gameSessionManager;
		}

		public async Task<GameSessionResponse> Handle(GetGameState request, CancellationToken cancellationToken)
		{
			var result = _gameSessionManager.GetGameSession(request.Id);
			if (!result.IsSuccess)
			{
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error }
				};
			}

			return new GameSessionResponse()
			{
				Success = true,
				GameSession = new GameSessionDto()
				{
					Id = result.Value.Id,
					Players = result.Value.Players,
					GameStatus = result.Value.GameStatus.ToString(),
					Map = result.Value.Map
				}
			};
		}
	}
}
