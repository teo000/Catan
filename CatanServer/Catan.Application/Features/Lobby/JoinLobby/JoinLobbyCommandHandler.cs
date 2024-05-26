using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.Responses;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Lobby.JoinLobby
{
	public class JoinLobbyCommandHandler : IRequestHandler<JoinLobbyCommand, LobbyPlayerResponse>
	{
		private LobbyManager _lobbyManager;
		private IMapper _mapper;

		public JoinLobbyCommandHandler(LobbyManager lobbyManager, IMapper mapper)
		{
			_lobbyManager = lobbyManager;
			_mapper = mapper;
		}

		public async Task<LobbyPlayerResponse> Handle(JoinLobbyCommand request, CancellationToken cancellationToken)
		{
			var playerResult = Player.Create(request.PlayerName);
			if (!playerResult.IsSuccess)
			{
				return new LobbyPlayerResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { playerResult.Error }
				};
			}

			var player = playerResult.Value;
			var lobbyResult = _lobbyManager.Join(request.JoinCode, player);

			if (!lobbyResult.IsSuccess)
			{
				return new LobbyPlayerResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { lobbyResult.Error }
				};
			}

			return new LobbyPlayerResponse()
			{
				Success = true,
				Lobby = _mapper.Map<LobbyDto>(lobbyResult.Value),
				PlayerId = playerResult.Value.Id
			};
		}
	}
}
