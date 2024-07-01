using Catan.API.Hubs;
using Catan.Application.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Catan.API.Notifiers
{
	public class SignalRGameNotifier : Application.Contracts.IGameNotifier
	{
		private readonly IHubContext<GameHub> _hubContext;

		public SignalRGameNotifier(IHubContext<GameHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task NotifyGameAsync(GameSessionDto session)
		{
			await _hubContext.Clients.Group(session.Id.ToString()).SendAsync("ReceiveGame", session);
		}
	}
}
