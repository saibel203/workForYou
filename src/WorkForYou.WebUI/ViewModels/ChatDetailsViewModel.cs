using WorkForYou.Core.Models;

namespace WorkForYou.WebUI.ViewModels;

public class ChatDetailsViewModel
{
    public string SenderUsername { get; set; } = string.Empty;
    public string RecipientUsername { get; set; } = string.Empty;
    public ChatRoom? ChatDetails { get; set; }
}
