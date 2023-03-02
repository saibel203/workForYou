namespace WorkForYou.Core.Models;

public class ChatRoom
{
    public int ChatRoomId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
}
