using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Queries.GetGameState
{
    public class GetGameStateHandler : IRequestHandler<GetGameState, GameSessionResponse>
	{
		private GameSessionManager _gameSessionManager;
		private IMapper _mapper;
		public GetGameStateHandler(GameSessionManager gameSessionManager, IMapper mapper)
		{
			_gameSessionManager = gameSessionManager;
			_mapper = mapper;
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
				GameSession = _mapper.Map<GameSessionDto>(result.Value)
				
				//GameSession = new GameSessionDto()
				//{
				//	Id = result.Value.Id,
				//	Players = result.Value.Players,
				//	GameStatus = result.Value.GameStatus.ToString(),
				//	Map = result.Value.GameMap,
				//	TurnPlayerIndex = result.Value.TurnPlayerIndex,
				//	TurnEndTime = result.Value.TurnEndTime,
				//}
			};
		}
	}
}
