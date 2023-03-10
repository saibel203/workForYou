using WorkForYou.Core.DTOModels.ChatDTOs;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IChatRepository
{
    Task<ChatResponse> CreateChatRoomAsync(string chatName, string currentUserId, string converseUserId);
    Task<ChatResponse> RemoveChatRoomAsync(int chatRoomId);
    Task<ChatResponse> GetChatDetailsAsync(int chatId, string userRole, string currentUserId);
    Task<ChatResponse> GetChatDetailsByChatNameAsync(string chatName);
    Task<ChatResponse> GetAllUserChatsAsync(string userId, string userRole);
    Task<ChatResponse> CreateChatMessageAsync(CreateMessageDto? createMessageDto, string currentUsername);
}