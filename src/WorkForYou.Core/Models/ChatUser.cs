using WorkForYou.Core.Enums;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Core.Models;

public class ChatUser
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }

    public int ChatRoomId { get; set; }
    public ChatRoom? ChatRoom { get; set; }
    public ChatUserRole Role { get; set; }
}
