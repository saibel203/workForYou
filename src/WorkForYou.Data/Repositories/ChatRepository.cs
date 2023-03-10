using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.ChatDTOs;
using WorkForYou.Core.Enums;
using WorkForYou.Core.Models;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class ChatRepository : GenericRepository<ChatRoom>, IChatRepository
{
    public ChatRepository(WorkForYouDbContext context, ILogger logger) : base(context, logger)
    {
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
            
            if (userRole == ApplicationRoles.EmployerRole)
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
            
            var opponentData = await Context.ChatUsers
                .FirstOrDefaultAsync(chatUserData => chatUserData.ChatRoomId == chatId 
                                                     && chatUserData.ApplicationUserId != currentUserId);

            if (opponentData is null)
                return new()
                {
                    Message = "Error getting the opposite user",
                    IsSuccessfully = false
                };
            
            var isThisUsers = chatDetails.ChatUsers
                .Any(chatData => chatData.ChatRoomId == chatId && chatData.ApplicationUserId == currentUserId);
            var isOpponentUser = chatDetails.ChatUsers
                .Any(chatData => chatData.ChatRoomId == chatId && chatData.ApplicationUserId == opponentData.ApplicationUserId);

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

    public async Task<ChatResponse> GetChatDetailsByChatNameAsync(string chatName)
    {
        try
        {
            var chatDetails = await DbSet
                .FirstOrDefaultAsync(chatData => chatData.Name == chatName);

            if (chatDetails is null)
                return new()
                {
                    Message = "Chat with given name not found",
                    IsSuccessfully = false
                };

            return new()
            {
                Message = "Chat received successfully",
                IsSuccessfully = true,
                ChatRoom = chatDetails
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while receiving the chat");
            return new()
            {
                Message = "An error occurred while receiving the chat",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> GetAllUserChatsAsync(string userId, string userRole)
    {
        try
        {
            List<ChatRoom?> chats;

            if (userRole == ApplicationRoles.EmployerRole)
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

    public async Task<ChatResponse> CreateChatMessageAsync(CreateMessageDto? createMessageDto, 
        string currentUsername)
    {
        try
        {
            if (createMessageDto is null)
                return new()
                {
                    Message = "An error occurred when receiving data about a new message",
                    IsSuccessfully = false
                };
            
            var chatMessage = new ChatMessage
            {
                ChatRoomId = createMessageDto.RoomId,
                Content = createMessageDto.MessageContent,
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