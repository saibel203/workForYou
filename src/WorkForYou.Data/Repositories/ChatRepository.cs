using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.Enums;
using WorkForYou.Core.Models;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class ChatRepository : GenericRepository<ChatRoom>, IChatRepository
{
    private const string EmployerRole = "employer";

    private readonly IChatService _chatService;
    
    public ChatRepository(WorkForYouDbContext context, ILogger logger, IChatService chatService) : base(context, logger)
    {
        _chatService = chatService;
    }

    public async Task<ChatResponse> CreateChatRoomAsync(string chatName, string currentUserId, string converseUserId)
    {
        try
        {
            var chat = new ChatRoom
            {
                Name = chatName
            };

            var currentChatUser = new ChatUser
            {
                ApplicationUserId = currentUserId,
                Role = ChatUserRole.Admin
            };

            var converseChatUser = new ChatUser
            {
                ApplicationUserId = converseUserId,
                Role = ChatUserRole.Member
            };

            chat.ChatUsers.Add(currentChatUser);
            chat.ChatUsers.Add(converseChatUser);

            await Context.AddAsync(chat);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "Chat successfully created",
                IsSuccessfully = true,
                ChatRoom = chat
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while trying to create a chat");
            return new()
            {
                Message = "An error occurred while trying to create a chat",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> RemoveChatRoomAsync(int chatRoomId)
    {
        try
        {
            var chatRoom = await DbSet
                .FirstOrDefaultAsync(chatData => chatData.ChatRoomId == chatRoomId);

            if (chatRoom is null)
                return new()
                {
                    Message = "Error receiving chat",
                    IsSuccessfully = false
                };
            
            Context.ChatRooms.Remove(chatRoom);
            await Context.SaveChangesAsync();
            
            return new()
            {
                Message = "Chat deleted successfully",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error trying to delete chat");
            return new()
            {
                Message = "Error trying to delete chat",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> GetChatDetailsAsync(int chatId, string userRole, string currentUserId)
    {
        try
        {
            ChatRoom? chatDetails;
            
            if (userRole == EmployerRole)
                chatDetails = await Context.ChatUsers
                    .Include(chatUserData => chatUserData.ChatRoom)
                    .ThenInclude(chatUserData => chatUserData!.ChatUsers)
                    .ThenInclude(chatUserData => chatUserData.ApplicationUser)
                    .ThenInclude(chatUserData => chatUserData!.CandidateUser)
                    .Include(chatUserData => chatUserData.ApplicationUser)
                    .Include(chatUserData => chatUserData.ChatRoom!.ChatMessages)
                    .AsSplitQuery()
                    .Select(chatData => chatData.ChatRoom)
                    .FirstOrDefaultAsync(chatData => chatData!.ChatRoomId == chatId);
            else 
                chatDetails = await Context.ChatUsers
                    .Include(chatUserData => chatUserData.ChatRoom)
                    .ThenInclude(chatUserData => chatUserData!.ChatUsers)
                    .ThenInclude(chatUserData => chatUserData.ApplicationUser)
                    .ThenInclude(chatUserData => chatUserData!.EmployerUser)
                    .Include(chatUserData => chatUserData.ApplicationUser)
                    .Include(chatUserData => chatUserData.ChatRoom!.ChatMessages)
                    .AsSplitQuery()
                    .Select(chatData => chatData.ChatRoom)
                    .FirstOrDefaultAsync(chatData => chatData!.ChatRoomId == chatId);

            if (chatDetails is null)
                return new()
                {
                    Message = "Error retrieving chat data",
                    IsSuccessfully = false
                };

            var opponentResult = await _chatService.OpponentNameAsync(chatId, currentUserId);

            var isThisUsers = chatDetails.ChatUsers
                .Any(chatData => chatData.ChatRoomId == chatId && chatData.ApplicationUserId == currentUserId);
            var isOpponentUser = chatDetails.ChatUsers
                .Any(chatData => chatData.ChatRoomId == chatId && chatData.ApplicationUserId == opponentResult.OpponentUserId);

            if (!isThisUsers || !isOpponentUser)
                return new()
                {
                    Message = "You are not the owner of this chat",
                    IsSuccessfully = false
                };
            
            return new()
            {
                Message = "Error retrieving chat data",
                IsSuccessfully = true,
                ChatRoom = chatDetails
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving chat data");
            return new()
            {
                Message = "Error retrieving chat data",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> GetAllUserChatsAsync(string userId, string userRole)
    {
        try
        {
            List<ChatRoom?> chats;

            if (userRole == EmployerRole)
                chats = await Context.ChatUsers
                    .Include(chatUserData => chatUserData.ChatRoom)
                    .ThenInclude(chatUserData => chatUserData!.ChatUsers)
                    .ThenInclude(chatUserData => chatUserData.ApplicationUser)
                    .ThenInclude(chatUserData => chatUserData!.CandidateUser)
                    .Include(chatUserData => chatUserData.ApplicationUser)
                    .Where(chatData => chatData.ApplicationUserId == userId)
                    .Select(chatData => chatData.ChatRoom)
                    .ToListAsync();
            else 
                chats = await Context.ChatUsers
                    .Include(chatUserData => chatUserData.ChatRoom)
                    .ThenInclude(chatUserData => chatUserData!.ChatUsers)
                    .ThenInclude(chatUserData => chatUserData.ApplicationUser)
                    .ThenInclude(chatUserData => chatUserData!.EmployerUser)
                    .Include(chatUserData => chatUserData.ApplicationUser)
                    .Where(chatData => chatData.ApplicationUserId == userId)
                    .Select(chatData => chatData.ChatRoom)
                    .ToListAsync();

            return new()
            {
                Message = "User chats received successfully",
                IsSuccessfully = true,
                ChatRooms = chats!
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving user chats");
            return new()
            {
                Message = "Error retrieving user chats",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> CreateChatMessageAsync(string messageContent, string currentUsername,
        int roomId)
    {
        try
        {
            var chatMessage = new ChatMessage
            {
                ChatRoomId = roomId,
                Content = messageContent,
                Name = currentUsername,
                SendTime = DateTime.Now
            };

            await Context.ChatMessages.AddAsync(chatMessage);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "Message successfully created",
                IsSuccessfully = true,
                ChatMessage = chatMessage
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating message");
            return new()
            {
                Message = "Error creating message",
                IsSuccessfully = false
            };
        }
    }
}