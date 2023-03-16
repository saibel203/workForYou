using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Models;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.Helpers;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class RespondedListRepository : GenericRepository<RespondedList>, IRespondedListRepository
{
    private const int PageSize = 7;

    public RespondedListRepository(WorkForYouDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public async Task<RespondedListResponse> RespondToVacancyAsync(UsernameDto? usernameDto, int vacancyId)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var user = await Context.Users.FirstOrDefaultAsync(userData => userData.UserName == usernameDto.Username);
            var vacancy =
                await Context.Vacancies.FirstOrDefaultAsync(vacancyData => vacancyData.VacancyId == vacancyId);

            if (vacancy is null)
                return new()
                {
                    Message = "Vacancy not found",
                    IsSuccessfully = false
                };

            if (user is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            const string candidateRole = "candidate";

            if (usernameDto.UserRole != candidateRole)
                return new()
                {
                    Message = "The user may not be a candidate",
                    IsSuccessfully = false
                };

            var response = new RespondedList
            {
                ApplicationUserId = user.Id,
                VacancyId = vacancyId
            };

            await DbSet.AddAsync(response);
            vacancy.ReviewsCount++;

            await Context.SaveChangesAsync();

            return new()
            {
                Message = "You have successfully responded",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "There was an error while trying to respond to the vacancy");
            return new()
            {
                Message = "There was an error while trying to respond to the vacancy",
                IsSuccessfully = false
            };
        }
    }

    public async Task<RespondedListResponse> RemoveRespondToVacancyAsync(UsernameDto? usernameDto, int vacancyId)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var user = await Context.Users.FirstOrDefaultAsync(userData => userData.UserName == usernameDto.Username);
            var vacancy =
                await Context.Vacancies.FirstOrDefaultAsync(vacancyData => vacancyData.VacancyId == vacancyId);

            if (vacancy is null)
                return new()
                {
                    Message = "Vacancy not found",
                    IsSuccessfully = false
                };

            if (user is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var response = new RespondedList
            {
                ApplicationUserId = user.Id,
                VacancyId = vacancyId
            };

            DbSet.Remove(response);
            vacancy.ReviewsCount--;

            await Context.SaveChangesAsync();

            return new()
            {
                Message = "You have successfully canceled your review",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while trying to cancel a job review");
            return new()
            {
                Message = "An error occurred while trying to cancel a job review",
                IsSuccessfully = false
            };
        }
    }

    public async Task<VacancyResponse> AllCandidateRespondedAsync(UsernameDto? usernameDto,
        QueryParameters queryParameters)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error retrieving the list of jobs you responded to",
                    IsSuccessfully = false
                };

            var userData = await Context.Users
                .FirstOrDefaultAsync(userData => userData.UserName == usernameDto.Username);

            if (userData is null || userData.CandidateUser is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            int skipAmount = PageSize * (queryParameters.PageNumber - 1);

            var vacancies = DbSet
                .Include(respondData => respondData.ApplicationUser)
                .Where(respondData => respondData.ApplicationUserId == userData.Id)
                .Select(respondData => respondData.Vacancy!).AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                vacancies = vacancies.Where(vacancyData =>
                    EF.Functions.Like(vacancyData.VacancyTitle, $"%{queryParameters.SearchString}%"));

            vacancies = ListSortingHelper.FavouriteListVacancySort(queryParameters, vacancies);
            vacancies = FilteringHelper.RespondedListVacanciesFiltering(queryParameters, vacancies);

            var vacanciesCount = vacancies.Count();
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / PageSize);

            return new()
            {
                Message = "Error retrieving the list of jobs you responded to",
                IsSuccessfully = true,
                VacancyList = vacancies.ToList()
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
            Logger.LogError(ex, "Error retrieving the list of jobs you responded to");
            return new()
            {
                Message = "Error retrieving the list of jobs you responded to",
                IsSuccessfully = false
            };
        }
    }

    public async Task<RespondedListResponse> IsVacancyInRespondedListAsync(string userId, int vacancyId)
    {
        try
        {
            var isVacancyInRespondedList = await DbSet
                .AnyAsync(respondedData =>
                    respondedData.ApplicationUserId == userId && respondedData.VacancyId == vacancyId);

            if (isVacancyInRespondedList)
                return new()
                {
                    Message = "The vacancy is present in the list of reviews",
                    IsSuccessfully = true,
                    IsVacancyInFavouriteList = true
                };

            return new()
            {
                Message = "The vacancy is not in the list of reviews",
                IsSuccessfully = true,
                IsVacancyInFavouriteList = false
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving feedback list");
            return new()
            {
                Message = "Error retrieving feedback list",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> AllVacancyResponses(int vacancyId, QueryParameters queryParameters)
    {
        try
        {
            var vacancyData =
                await Context.Vacancies.FirstOrDefaultAsync(vacancyData => vacancyData.VacancyId == vacancyId);

            if (vacancyData is null)
                return new()
                {
                    Message = "",
                    IsSuccessfully = false
                };

            int skipAmount = PageSize * (queryParameters.PageNumber - 1);

            var users = DbSet
                .Include(x => x.Vacancy)
                .Include(x => x.ApplicationUser!.CandidateUser)
                .Where(x => x.ApplicationUser!.CandidateUser!.IsProfileComplete)
                .Where(x => x.VacancyId == vacancyId)
                .Select(x => x.ApplicationUser!).AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                users = users
                    .Where(x => EF.Functions.Like(x.CandidateUser!.CompanyPosition!,
                        $"%{queryParameters.SearchString}%"));

            users = ListSortingHelper.VacancyResponsesListSort(queryParameters, users);
            users = FilteringHelper.ResponsesListCandidateFiltering(queryParameters, users);

            var vacanciesCount = users.Count();
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / PageSize);

            return new()
            {
                Message = "FavouriteList successfully received",
                IsSuccessfully = true,
                ApplicationUsers = users.ToList()
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
            Logger.LogError(ex, "");
            return new()
            {
                Message = "",
                IsSuccessfully = false
            };
        }
    }
}