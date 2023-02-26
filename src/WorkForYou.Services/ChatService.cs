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
    
    public async Task<ChatResponse> IsChatExists(int candidateId, int employerId)
    {
        try
        {
            var existsResult = await _context.ChatRooms
                .AnyAsync(x => x.CandidateUserId == candidateId && x.EmployerUserId == employerId);

            if (existsResult)
                return new()
                {
                    Message = "The room exists",
                    IsSuccessfully = true,
                    IsChatExists = true
                };

            return new()
            {
                Message = "The not room exists",
                IsSuccessfully = true,
                IsChatExists = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user");
            return new()
            {
                Message = "Error getting user",
                IsSuccessfully = false
            };
        }
    }

}
