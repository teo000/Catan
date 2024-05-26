using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.GetLobby
{
	public record GetLobbyQuery (string JoinCode) : IRequest<LobbyResponse>
	{
	}

}
