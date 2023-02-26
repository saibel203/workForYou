namespace WorkForYou.Core.Models;

public class ChatRoom
{
    public int ChatRoomId { get; set; }
    public int CandidateUserId { get; set; }
    public int EmployerUserId { get; set; }
    public CandidateUser? CandidateUser { get; set; }
    public EmployerUser? EmployerUser { get; set; }
    public ICollection<ChatMessage>? ChatMessages { get; set; }

    public ChatRoom()
    {
        ChatMessages = new List<ChatMessage>();
    }
}
