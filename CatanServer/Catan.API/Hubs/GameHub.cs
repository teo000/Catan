using Microsoft.AspNetCore.SignalR;

namespace Catan.API.Hubs;

public class GameHub : Hub
{
	public async Task JoinGroup(string groupName)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
	}

	public async Task LeaveGroup(string groupName)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
	}
}
