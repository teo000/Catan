using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.CreateLobby
{
    public class CreateLobbyCommand : IRequest<LobbyPlayerResponse>
	{
		public string PlayerName { get; set; }
	}
}
