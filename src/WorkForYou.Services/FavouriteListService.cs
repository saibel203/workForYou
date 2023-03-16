using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Data.Helpers;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Services;

public class FavouriteListService : IFavouriteListService
{
    private readonly IStringLocalizer<FavouriteListService> _stringLocalization;
    private readonly ILogger<FavouriteListService> _logger;
    private readonly WorkForYouDbContext _context;

    private const int PageSize = 7;

    public FavouriteListService(ILogger<FavouriteListService> logger, WorkForYouDbContext context,
        IStringLocalizer<FavouriteListService> stringLocalization)
    {
        _logger = logger;
        _context = context;
        _stringLocalization = stringLocalization;
    }

    public async Task<FavouriteListResponse> GetFavouriteVacancyListAsync(UsernameDto? usernameDto,
        QueryParameters queryParameters)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var user = await _context.Users
                .Include(x => x.CandidateUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user?.CandidateUser is null)
                return new()
                {
                    Message = "An error occurred while retrieving the user, or the user is not a candidate",
                    IsSuccessfully = false
                };

            int skipAmount = PageSize * (queryParameters.PageNumber - 1);

            var vacancies = _context.FavouriteVacancies
                .Include(x => x.CandidateUser)
                .Where(x => x.CandidateId == user.CandidateUser.CandidateUserId)
                .Select(x => x.Vacancy!).AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                vacancies = vacancies
                    .Where(x => EF.Functions.Like(x.VacancyTitle, $"%{queryParameters.SearchString}%"));

            vacancies = ListSortingHelper.FavouriteListVacancySort(queryParameters, vacancies);
            vacancies = FilteringHelper.RespondedListVacanciesFiltering(queryParameters, vacancies);

            var vacanciesCount = vacancies.Count();
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / PageSize);

            return new()
            {
                Message = "FavouriteList successfully received",
                IsSuccessfully = true,
                FavouriteVacancyList = vacancies.ToList()
                    .Skip(skipAmount)
                    .Take(PageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = queryParameters.SearchString,
                Pages = PaginationHelper.PageNumbers(queryParameters.PageNumber, pageCount),
                SortBy = queryParameters.SortBy
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving saved vacancy list");
            return new()
            {
                Message = "Error retrieving saved vacancy list",
                IsSuccessfully = false
            };
        }
    }

    public async Task<FavouriteListResponse> GetFavouriteCandidateListAsync(UsernameDto? usernameDto,
        QueryParameters queryParameters)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var user = await _context.Users.Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user is null)
                return new()
                {
                    Message = "An error occurred while retrieving the user, or the user is not a employer",
                    IsSuccessfully = false
                };

            var skipAmount = PageSize * (queryParameters.PageNumber - 1);

            var users = _context.FavouriteCandidates
                .Include(x => x.EmployerUser)
                .Where(x => x.EmployerUserId == user.EmployerUser!.EmployerUserId)
                .Include(x => x.CandidateUser!.ApplicationUser)
                .Select(x => x.CandidateUser!).AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                users = users
                    .Where(x => EF.Functions.Like(x.CompanyPosition!, $"%{queryParameters.SearchString}%"));

            users = ListSortingHelper.FavouriteListUserSort(queryParameters, users);
            users = FilteringHelper.FavouriteCandidateFiltering(queryParameters, users);

            var vacanciesCount = users.Count();
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / PageSize);

            return new()
            {
                Message = "FavouriteList successfully received",
                IsSuccessfully = true,
                FavouriteCandidateList = users.ToList()
                    .Skip(skipAmount)
                    .Take(PageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = queryParameters.SearchString,
                Pages = PaginationHelper.PageNumbers(queryParameters.PageNumber, pageCount),
                SortBy = queryParameters.SortBy
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving saved user list");
            return new()
            {
                Message = "Error retrieving saved user list",
                IsSuccessfully = false
            };
        }
    }

    public async Task<FavouriteListResponse> AddCandidateToFavouriteListAsync(UsernameDto? usernameDto, int candidateId)
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

            var isCandidateInList =
                await IsCandidateInFavouriteListAsync(employerUser.EmployerUser.EmployerUserId, candidateId);

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
    
    public async Task<FavouriteListResponse> IsCandidateInFavouriteListAsync(int employerUserId, int candidateUserId)
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
    
    public async Task<FavouriteListResponse> AddVacancyToFavouriteListAsync(UsernameDto? usernameDto, int vacancyId)
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

            if (user?.CandidateUser is null)
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
                vacancy.FavouriteCount--;
                _context.FavouriteVacancies.Remove(favourite);
                await _context.SaveChangesAsync();

                return new()
                {
                    Message = _stringLocalization["RemoveFromListSuccess"],
                    IsSuccessfully = true
                };
            }

            vacancy.FavouriteCount++;
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

    public async Task<FavouriteListResponse> IsVacancyInFavouriteListAsync(int vacancyId, int candidateId)
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