using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WorkForYou.WebUI.Hubs;

[Authorize]
public class ChatHub : Hub
{
    // public override Task OnConnectedAsync()
    // {
    //     // var currentRole = Context.User?.IsInRole("employer");
    //     //
    //     var currentUser = Context.User?.Identity?.Name!;
    //     var connectionId = Context.ConnectionId;
    //     //
    //     // string recipientUser;
    //     //
    //     // if (currentRole == true)
    //     //     recipientUser = Context.GetHttpContext()?.Request.Query["candidateUsername"]!;
    //     // else
    //     //     recipientUser = Context.GetHttpContext()?.Request.Query["employerUsername"]!;
    //     
    //     Groups.AddToGroupAsync(connectionId, currentUser);
    //     return base.OnConnectedAsync();
    // }

    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public Task SendMessageToGroup(string sender, string recipient, string message)
    {
        return Clients.Group(recipient).SendAsync("ReceiveMessage", sender, message);
    }
}