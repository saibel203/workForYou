namespace WorkForYou.Core.Models;

public class ChatMessage
{
    public int ChatMessageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime SendTime { get; set; } = DateTime.Now;
    public int ChatRoomId { get; set; }
    public ChatRoom? ChatRoom { get; set; }
}
