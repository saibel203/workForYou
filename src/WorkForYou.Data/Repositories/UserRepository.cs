using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.DtoModels;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(WorkForYouDbContext context, ILogger logger, IHttpContextAccessor httpContextAccessor, 
        UserManager<ApplicationUser> userManager, IMapper mapper) 
        : base(context, logger, httpContextAccessor, userManager, mapper)
    {
    }

    public async Task<UserResponse> GetUserDataAsync(UsernameDto? usernameDto)
    {
        try
        {
            if (usernameDto is null)
            {
                return new()
                {
                    Message = "Data transfer error",
                    IsSuccessfully = false
                };
            }
        
            var user = await DbSet.FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user is null)
                return new()
                {
                    Message = "No user with this ID was found",
                    IsSuccessfully = false
                };


            var userRole = await UserManager.IsInRoleAsync(user, "candidate");

            if (userRole)
            {
                var candidateUser = await DbSet
                    .Include(x => x.CandidateUser)
                    .Include(x => x.CandidateUser!.CategoryWork)
                    .Include(x => x.CandidateUser!.CommunicationLanguage)
                    .Include(x => x.CandidateUser!.LevelEnglish)
                    .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

                if (candidateUser is null)
                    return new()
                    {
                        Message = "Error getting candidate",
                        IsSuccessfully = false
                    };

                return new()
                {
                    Message = "The candidate was successfully received",
                    IsSuccessfully = true,
                    User = candidateUser
                };
            }

            var employerUser = await DbSet.Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (employerUser is null)
                return new()
                {
                    Message = "Error getting employer",
                    IsSuccessfully = false
                };

            return new()
            {
                Message = "The employer was successfully received",
                IsSuccessfully = true,
                User = employerUser
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Data transfer error");
            return new()
            {
                Message = "Data transfer error",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> ShowFavouriteListAsync(UsernameDto? usernameDto)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Data transfer error",
                    IsSuccessfully = false
                };
        
            var user = await DbSet.Include(x => x.CandidateUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user is null || user.CandidateUser is null)
                return new()
                {
                    Message = "An error occurred while retrieving the user, or the user is not a candidate",
                    IsSuccessfully = false
                };

            var favouriteProperties = await Context.FavouriteVacancies
                .Include(x => x.CandidateUser)
                .Where(x => x.CandidateId == user.CandidateUser.CandidateUserId)
                .Select(x => x.Vacancy).ToListAsync();

            return new()
            {
                Message = "FavouriteList successfully received",
                IsSuccessfully = true,
                FavouriteList = favouriteProperties
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Data transfer error");
            return new()
            {
                Message = "Data transfer error",
                IsSuccessfully = false
            };
        }
    }
}
