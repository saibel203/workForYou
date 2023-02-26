using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Models;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class ChatRepository : GenericRepository<ChatRoom>, IChatRepository
{
    
    public ChatRepository(WorkForYouDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public async Task<ChatResponse> CreateChatAsync(UsernameDto? candidateUsername, UsernameDto? employerUsername)
    {
        try
        {
            if (candidateUsername is null || employerUsername is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var candidateUser = await Context.Users
                .Include(x => x.CandidateUser)
                .FirstOrDefaultAsync(userData => userData.UserName == candidateUsername.Username);
            var employerUser = await Context.Users
                .Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(userData => userData.UserName == employerUsername.Username);

            if (candidateUser is null || employerUser is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            if (candidateUser.CandidateUser is null || employerUser.EmployerUser is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            // var isCandidate = await _authService.IsUserCandidate(candidateUsername);
            // var isEmployer = await _authService.IsUserCandidate(employerUsername);

            // if (!isCandidate.IsUserCandidate || isEmployer.IsUserCandidate)
            //     return new()
            //     {
            //         Message = "Error defining user role",
            //         IsSuccessfully = false
            //     };

            var chatRoom = new ChatRoom
            {
                CandidateUserId = candidateUser.CandidateUser.CandidateUserId,
                EmployerUserId = employerUser.EmployerUser.EmployerUserId
            };

            await DbSet.AddAsync(chatRoom);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "The room has been successfully created",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting user");
            return new()
            {
                Message = "Error getting user",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> GetChatDataAsync(int employerId, int candidateId)
    {
        try
        {
            var chatData = await DbSet
                .Include(x => x.ChatMessages)
                .FirstOrDefaultAsync(chatData => chatData.EmployerUserId == employerId
                    && chatData.CandidateUserId == candidateId);
            
            return new()
            {
                Message = "",
                IsSuccessfully = true,
                ChatRoom = chatData
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "");
            return new()
            {
                Message = "",
                IsSuccessfully = false
            };
        }
    }
    
    public async Task<ChatResponse> GetAllEmployerChatsAsync(UsernameDto? employerUsername)
    {
        try
        {
            if (employerUsername is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var employer = await Context.Users
                .Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(userData => userData.UserName == employerUsername.Username);

            if (employer is null || employer.EmployerUser is null)
                return new()
                {
                    Message = "Error getting user or user is not employer",
                    IsSuccessfully = false
                };

            var employerChats = await DbSet
                .Include(x => x.EmployerUser)
                .Include(x => x.EmployerUser!.ApplicationUser)
                .Where(x => x.EmployerUser!.ApplicationUser!.UserName == employerUsername.Username)
                .Select(x => x.CandidateUser!.ApplicationUser!).ToListAsync();

            return new()
            {
                Message = "Employer chats successfully get",
                IsSuccessfully = true,
                EmployerChats = employerChats
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting user");
            return new()
            {
                Message = "Error getting user",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> GetAllCandidateChatsAsync(UsernameDto? usernameDto)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };
            
            var candidate = await Context.Users
                .Include(x => x.CandidateUser)
                .FirstOrDefaultAsync(userData => userData.UserName == usernameDto.Username);

            if (candidate is null || candidate.CandidateUser is null)
                return new()
                {
                    Message = "Error getting user or user is not employer",
                    IsSuccessfully = false
                };

            var candidateChats = await DbSet
                .Include(x => x.CandidateUser)
                .Include(x => x.CandidateUser!.ApplicationUser)
                .Where(x => x.CandidateUser!.ApplicationUser!.UserName == usernameDto.Username)
                .Select(x => x.EmployerUser!.ApplicationUser!).ToListAsync();

            return new()
            {
                Message = "Employer chats successfully get",
                IsSuccessfully = true,
                CandidateChats = candidateChats
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting user");
            return new()
            {
                Message = "Error getting user",
                IsSuccessfully = false
            };
        }
    }

    public async Task<ChatResponse> CreateMessageAsync(string content, int roomId, string applicationUserId)
    {
        try
        {
            var chatMessage = new ChatMessage
            {
                Content = content,
                ChatRoomId = roomId,
                ApplicationUserId = applicationUserId
            };

            await Context.ChatMessages.AddAsync(chatMessage);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "",
                IsSuccessfully = true,
                ChatMessage = chatMessage
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "");
            return new()
            {
                Message = "",
                IsSuccessfully = false
            };
        }
    }
}
