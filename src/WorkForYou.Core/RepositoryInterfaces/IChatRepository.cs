using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IChatRepository
{
    Task<ChatResponse> CreateChatAsync(UsernameDto? candidateUsername, UsernameDto? employerUsername);
    Task<ChatResponse> GetChatDataAsync(int candidateId, int employerId);
    Task<ChatResponse> GetAllEmployerChatsAsync(UsernameDto? employerUsername);
    Task<ChatResponse> GetAllCandidateChatsAsync(UsernameDto candidateUsername);
    Task<ChatResponse> CreateMessageAsync(string content, int roomId, string applicationUserId);
}