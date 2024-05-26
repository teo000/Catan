using MediatR;
using Catan.Application.Responses;
using AutoMapper;
using Catan.Application.Dtos;

namespace Catan.Application.Features.Lobby.GetLobby
{
	public class GetLobbyQueryHandler : IRequestHandler<GetLobbyQuery, LobbyResponse>
	{
		private LobbyManager _lobbyManager;
		private IMapper _mapper;

		public GetLobbyQueryHandler(LobbyManager lobbyManager, IMapper mapper)
		{
			_lobbyManager = lobbyManager;
			_mapper = mapper;
		}

		public async Task<LobbyResponse> Handle(GetLobbyQuery request, CancellationToken cancellationToken)
		{
			var lobbyResult = _lobbyManager.Get(request.JoinCode);

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
				Lobby = _mapper.Map<LobbyDto>(lobbyResult.Value),
			};
		}
	}
}
