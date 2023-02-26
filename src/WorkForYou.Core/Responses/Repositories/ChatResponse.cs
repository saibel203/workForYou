using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Core.Responses.Repositories;

public class ChatResponse : BaseResponse
{
    public IEnumerable<ApplicationUser>? EmployerChats { get; set; }
    public IEnumerable<ApplicationUser>? CandidateChats { get; set; }
    public ChatRoom? ChatRoom { get; set; }
    public ChatMessage? ChatMessage { get; set; }
}
