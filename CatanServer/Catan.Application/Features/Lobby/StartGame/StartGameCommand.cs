using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.StartGame
{
	public class StartGameCommand : IRequest<LobbyResponse>
	{
		public string JoinCode { get; set; }
	}
}
