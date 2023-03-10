using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Services;

public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly WorkForYouDbContext _context;

    public ChatService(ILogger<ChatService> logger, WorkForYouDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ChatResponse> IsChatExistsAsync(string chatName)
    {
        try
        {
            var isChatExists = await _context.ChatRooms
                .AnyAsync(chatRoomData => chatRoomData.Name == chatName);

            if (!isChatExists)
                return new()
                {
                    Message = "Chat does not exist",
                    IsSuccessfully = true,
                    IsChatExists = false
                };

            return new()
            {
                Message = "Chat exists",
                IsSuccessfully = true,
                IsChatExists = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error receiving chat");
            return new()
            {
                Message = "Error receiving chat",
                IsSuccessfully = false
            };
        }
    }
}
