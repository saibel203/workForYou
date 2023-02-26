using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Services;

public class VacancyService : IVacancyService
{
    private readonly IStringLocalizer<VacancyService> _stringLocalization;
    private readonly ILogger<VacancyService> _logger;
    private readonly WorkForYouDbContext _context;

    public VacancyService(ILogger<VacancyService> logger, WorkForYouDbContext context, IStringLocalizer<VacancyService> stringLocalization)
    {
        _logger = logger;
        _context = context;
        _stringLocalization = stringLocalization;
    }

    public async Task<VacancyResponse> AddVacancyToFavouriteListAsync(UsernameDto? usernameDto, int vacancyId)  
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = _stringLocalization["GettingUserError"],
                    IsSuccessfully = false
                };
            
            var user = await _context.Users.Include(x => x.CandidateUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user is null || user.CandidateUser is null)
                return new()
                {
                    Message = _stringLocalization["UserNotCandidate"],
                    IsSuccessfully = false
                };

            var vacancy = await _context.Vacancies.FirstOrDefaultAsync(x => x.VacancyId == vacancyId);

            if (vacancy is null)
                return new()
                {
                    Message = _stringLocalization["ErrorGettingVacancy"],
                    IsSuccessfully = false
                };

            var isVacancyInListResult = await IsVacancyInFavouriteListAsync(vacancyId, user.CandidateUser.CandidateUserId);

            var favourite = new FavouriteVacancy
            {
                CandidateId = user.CandidateUser.CandidateUserId,
                VacancyId = vacancyId
            };

            if (isVacancyInListResult.IsVacancyInFavouriteList)
            {
                _context.FavouriteVacancies.Remove(favourite);
                await _context.SaveChangesAsync();

                return new()
                {
                    Message = _stringLocalization["RemoveFromListSuccess"],
                    IsSuccessfully = true
                };
            }

            await _context.FavouriteVacancies.AddAsync(favourite);
            await _context.SaveChangesAsync();

            return new()
            {
                Message = _stringLocalization["AddedToListSuccess"],
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user");
            return new()
            {
                Message = _stringLocalization["GettingUserError"],
                IsSuccessfully = false
            };
        }
    }

    public async Task<VacancyResponse> IsVacancyInFavouriteListAsync(int vacancyId, int candidateId)
    {
        try
        {
            var isVacancyInList = await _context.FavouriteVacancies
                .AnyAsync(x => x.VacancyId == vacancyId && x.CandidateId == candidateId);

            if (isVacancyInList)
                return new()
                {
                    Message = _stringLocalization["VacancyInList"],
                    IsSuccessfully = true,
                    IsVacancyInFavouriteList = true
                };

            return new()
            {
                Message = _stringLocalization["NoVacancyInList"],
                IsSuccessfully = true,
                IsVacancyInFavouriteList = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving a job or user");
            return new()
            {
                Message = _stringLocalization["ErrorGettingVacancyOrUser"],
                IsSuccessfully = false
            };
        }
    }
}
