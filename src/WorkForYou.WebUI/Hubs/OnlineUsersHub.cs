using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace WorkForYou.WebUI.Hubs;

public class OnlineUsersHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> OnlineUsers
        = new();

    public override async Task OnConnectedAsync()
    {
        OnlineUsers.TryAdd(Context.ConnectionId, Context.User?.Identity?.Name!);
        await UpdateOnlineUsers();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        OnlineUsers.TryRemove(Context.ConnectionId, out string? outRemove);
        await UpdateOnlineUsers();
        await base.OnDisconnectedAsync(exception);
    }

    public async Task UpdateOnlineUsers()
    {
        var onlineUsers = OnlineUsers.Values.Distinct().ToList();
        await Clients.All.SendAsync("UpdateOnlineUsers", onlineUsers);
    }
}