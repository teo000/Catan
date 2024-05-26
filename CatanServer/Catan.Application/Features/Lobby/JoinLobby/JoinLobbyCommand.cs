using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.JoinLobby
{
	public class JoinLobbyCommand : IRequest<LobbyPlayerResponse>
	{
		public string JoinCode { get; set; }
		public string PlayerName { get; set; }
	}
}
