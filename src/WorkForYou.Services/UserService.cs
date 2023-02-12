using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.IServices;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Services;

public class UserService : IUserService
{
    private readonly IStringLocalizer<UserService> _stringLocalization;
    private readonly ILogger<UserService> _logger;
    private readonly WorkForYouDbContext _context;

    public UserService(ILogger<UserService> logger, WorkForYouDbContext context, IStringLocalizer<UserService> stringLocalization)
    {
        _logger = logger;
        _context = context;
        _stringLocalization = stringLocalization;
    }

    public async Task<UserResponse> AddCandidateToFavouriteListAsync(UsernameDto? usernameDto, int candidateId)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = _stringLocalization["ErrorGettingUser"],
                    IsSuccessfully = false
                };

            var employerUser = await _context.Users
                .Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);
            
            if (employerUser is null || employerUser.EmployerUser is null)
                return new()
                {
                    Message = _stringLocalization["ErrorGettingUserOrUserNotEmployer"],
                    IsSuccessfully = false
                };

            var candidate = await _context.CandidateUsers
                .FirstOrDefaultAsync(x => x.CandidateUserId == candidateId);
            
            if (candidate is null)
                return new()
                {
                    Message = _stringLocalization["ErrorGettingUserOrUserNotCandidate"],
                    IsSuccessfully = false
                };

            var isCandidateInList = await IsCandidateInFavouriteListAsync(employerUser.EmployerUser.EmployerUserId, candidateId);

            var favourite = new FavouriteCandidate
            {
                EmployerUserId = employerUser.EmployerUser.EmployerUserId,
                CandidateUserId = candidateId
            };

            if (isCandidateInList.IsCandidateInFavouriteList)
            {
                _context.FavouriteCandidates.Remove(favourite);
                await _context.SaveChangesAsync();

                return new()
                {
                    Message = _stringLocalization["CandidateSuccessRemoveFromList"],
                    IsSuccessfully = true
                };
            }

            await _context.FavouriteCandidates.AddAsync(favourite);
            await _context.SaveChangesAsync();
            
            return new()
            {
                Message = _stringLocalization["CandidateSuccessAddToList"],
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user");
            return new()
            {
                Message = _stringLocalization["ErrorGettingUser"],
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> IsCandidateInFavouriteListAsync(int employerUserId, int candidateUserId)
    {
        try
        {
            var isCandidateInList = await _context.FavouriteCandidates
                .AnyAsync(x => x.EmployerUserId == employerUserId && x.CandidateUserId == candidateUserId);
            
            if (isCandidateInList)
                return new()
                {
                    Message = _stringLocalization["CandidateInList"],
                    IsSuccessfully = true,
                    IsCandidateInFavouriteList = true
                };

            return new()
            {
                Message = _stringLocalization["NoCandidateInList"],
                IsSuccessfully = true,
                IsCandidateInFavouriteList = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving a job or user");
            return new()
            {
                Message = _stringLocalization["ErrorCandidateInList"],
                IsSuccessfully = false
            };
        }
    }
}
