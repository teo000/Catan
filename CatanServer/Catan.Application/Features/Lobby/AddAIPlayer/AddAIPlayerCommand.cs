using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Lobby.AddAIPlayer
{
	public class AddAIPlayerCommand : IRequest<PlayerResponse>
	{
		public string JoinCode { get; set; }
	}
}
