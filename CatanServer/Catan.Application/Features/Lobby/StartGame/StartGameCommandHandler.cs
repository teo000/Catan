using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.StartGame
{
    public class StartGameCommandHandler : IRequestHandler<StartGameCommand, LobbyResponse>
	{
		private LobbyManager _lobbyManager;
		private IMapper _mapper;

		public StartGameCommandHandler(LobbyManager lobbyManager, IMapper mapper)
		{
			_lobbyManager = lobbyManager;
			_mapper = mapper;
		}

		public async Task<LobbyResponse> Handle(StartGameCommand request, CancellationToken cancellationToken)
		{
			var result = _lobbyManager.StartGame(request.JoinCode);
            if (!result.IsSuccess)
            {
				return new LobbyResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { result.Error },
				};
            }

			return new LobbyResponse()
			{
				Success = true,
				Lobby = _mapper.Map<LobbyDto>(result.Value)
			};
        }
	}
}
