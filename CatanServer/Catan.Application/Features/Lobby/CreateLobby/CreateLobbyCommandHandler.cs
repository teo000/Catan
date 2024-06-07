using MediatR;
using Catan.Application.Responses;
using AutoMapper;
using Catan.Domain.Entities;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;

namespace Catan.Application.Features.Lobby.CreateLobby
{
    public class CreateLobbyCommandHandler : IRequestHandler<CreateLobbyCommand, LobbyPlayerResponse>
	{
		private LobbyManager _lobbyManager;
		private IMapper _mapper;

		public CreateLobbyCommandHandler(LobbyManager lobbyManager, IMapper mapper)
		{
			_lobbyManager = lobbyManager;
			_mapper = mapper;
		}

		public async Task<LobbyPlayerResponse> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
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
			var lobbyResult = _lobbyManager.Create(player);

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
