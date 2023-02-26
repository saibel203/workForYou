using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IChatService
{
    Task<ChatResponse> IsChatExists(int candidateId, int employerId);
}