using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WorkForYou.WebUI.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public Task JoinToRoom(string roomId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    public Task LeaveRoom(string roomId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }
}