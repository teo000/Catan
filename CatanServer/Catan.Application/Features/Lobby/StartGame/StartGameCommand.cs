using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.StartGame
{
	public class StartGameCommand : IRequest<GameSessionResponse>
	{
		public string JoinCode { get; set; }
	}
}
