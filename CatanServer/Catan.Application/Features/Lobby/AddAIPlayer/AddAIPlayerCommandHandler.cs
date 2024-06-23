using MediatR;
using Catan.Application.Responses;
using Catan.Application.GameManagement;
using Catan.Application.Dtos;
using Catan.Domain.Entities;
using AutoMapper;

namespace Catan.Application.Features.Lobby.AddAIPlayer
{
	public class AddAIPlayerCommandHandler : IRequestHandler<AddAIPlayerCommand, LobbyResponse>
	{
		private readonly LobbyManager _lobbyManager;
		private readonly IMapper _mapper;

		public AddAIPlayerCommandHandler(LobbyManager lobbyManager, IMapper mapper)
		{
			_lobbyManager = lobbyManager;
			_mapper = mapper;
		}

		public async Task<LobbyResponse> Handle(AddAIPlayerCommand request, CancellationToken cancellationToken)
		{
			var getLobbyResult = _lobbyManager.Get(request.JoinCode);
			if (!getLobbyResult.IsSuccess)
			{
				return new LobbyResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { getLobbyResult.Error }
				};
			}
			var lobby = getLobbyResult.Value;
			var aINo = lobby.Players.Where(p => p.IsAI).Count();

			var playerResult = Player.CreateAI(aINo + 1);
			if (!playerResult.IsSuccess)
			{
				return new LobbyResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { playerResult.Error }
				};
			}

			var player = playerResult.Value;
			var lobbyResult = _lobbyManager.Join(request.JoinCode, player);

			if (!lobbyResult.IsSuccess)
			{
				return new LobbyResponse()
				{
					Success = false,
					ValidationErrors = new List<string>() { lobbyResult.Error }
				};
			}

			return new LobbyResponse()
			{
				Success = true,
				Lobby = _mapper.Map<LobbyDto>(lobby),
			};
		}
	}
}
