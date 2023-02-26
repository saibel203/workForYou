using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Core.Models;

public class ChatMessage
{
    public int ChatMessageId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SendTime { get; set; } = DateTime.Now;
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }
    public int ChatRoomId { get; set; }
    public ChatRoom? ChatRoom { get; set; }
}
