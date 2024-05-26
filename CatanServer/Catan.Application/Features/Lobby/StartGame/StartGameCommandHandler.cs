using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.StartGame
{
	public class StartGameCommandHandler : IRequestHandler<StartGameCommand, GameSessionResponse>
	{
		private LobbyManager _lobbyManager;
		private IMapper _mapper;

		public StartGameCommandHandler(LobbyManager lobbyManager, IMapper mapper)
		{
			_lobbyManager = lobbyManager;
			_mapper = mapper;
		}

		public async Task<GameSessionResponse> Handle(StartGameCommand request, CancellationToken cancellationToken)
		{
			var result = _lobbyManager.StartGame(request.JoinCode);
            if (!result.IsSuccess)
            {
				return new GameSessionResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error },
				};
            }

			return new GameSessionResponse()
			{
				Success = true,
				GameSession = _mapper.Map<GameSessionDto>(result.Value)
			};
        }
	}
}
