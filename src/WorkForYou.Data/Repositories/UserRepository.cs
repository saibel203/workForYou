using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.Helpers;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly IFileService _fileService;

    private const string CandidateRole = "candidate";
    private const int PageSize = 7;

    public UserRepository(WorkForYouDbContext context, ILogger logger,
        IFileService fileService)
        : base(context, logger)
    {
        _fileService = fileService;
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

            if (usernameDto.UserRole == CandidateRole)
            {
                var candidateUser = await DbSet
                    .Include(userData => userData.CandidateUser)
                    .Include(userData => userData.CandidateUser!.CategoryWork)
                    .Include(userData => userData.CandidateUser!.CommunicationLanguage)
                    .Include(userData => userData.CandidateUser!.LevelEnglish)
                    .FirstOrDefaultAsync(userData => userData.UserName == usernameDto.Username);

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

            var employerUser = await DbSet
                .Include(x => x.EmployerUser)
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

    public async Task<UserResponse> ShowFavouriteListAsync(UsernameDto? usernameDto, QueryParameters queryParameters)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var user = await DbSet.Include(x => x.CandidateUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user?.CandidateUser is null)
                return new()
                {
                    Message = "An error occurred while retrieving the user, or the user is not a candidate",
                    IsSuccessfully = false
                };

            int skipAmount = PageSize * (queryParameters.PageNumber - 1);

            var vacancies = Context.FavouriteVacancies
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
                FavouriteList = vacancies.ToList()
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
            Logger.LogError(ex, "Error retrieving saved user list");
            return new()
            {
                Message = "Error retrieving saved user list",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> ShowFavouriteCandidatesListAsync(UsernameDto? usernameDto,
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

            var user = await DbSet.Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user is null)
                return new()
                {
                    Message = "An error occurred while retrieving the user, or the user is not a employer",
                    IsSuccessfully = false
                };

            var skipAmount = PageSize * (queryParameters.PageNumber - 1);

            var users = Context.FavouriteCandidates
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
                FavouriteCandidates = users.ToList()
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
            Logger.LogError(ex, "Error retrieving saved user list");
            return new()
            {
                Message = "Error retrieving saved user list",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> GetAllCandidatesAsync(QueryParameters queryParameters)
    {
        try
        {
            var skipAmount = PageSize * (queryParameters.PageNumber - 1);

            IReadOnlyList<ApplicationUser> users;

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                users = await DbSet
                    .Where(x => x.CandidateUser != null && x.CandidateUser.IsProfileComplete)
                    .Include(x => x.CandidateUser!.CategoryWork)
                    .Include(x => x.CandidateUser!.LevelEnglish)
                    .Include(x => x.CandidateUser!.CommunicationLanguage)
                    .Where(x => EF.Functions.Like(x.CandidateUser!.CompanyPosition!,
                                    $"%{queryParameters.SearchString}%")
                                || EF.Functions.Like(x.CandidateUser!.KeyWords!, $"%{queryParameters.SearchString}%"))
                    .ToListAsync();
            else
                users = await DbSet
                    .Where(x => x.CandidateUser != null && x.CandidateUser.IsProfileComplete)
                    .Include(x => x.CandidateUser!.CategoryWork)
                    .Include(x => x.CandidateUser!.LevelEnglish)
                    .Include(x => x.CandidateUser!.CommunicationLanguage)
                    .ToListAsync();

            users = ListSortingHelper.ListUserSort(queryParameters, users);
            users = FilteringHelper.CandidateFiltering(queryParameters, users);

            var vacanciesCount = users.Count;
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / PageSize);

            return new()
            {
                Message = "Candidates successfully received",
                IsSuccessfully = true,
                ApplicationUsers = users
                    .Reverse()
                    .Skip(skipAmount)
                    .Take(PageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = queryParameters.SearchString,
                SortBy = queryParameters.SortBy,
                Pages = PaginationHelper.PageNumbers(queryParameters.PageNumber, pageCount)
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error receiving candidates");
            return new()
            {
                Message = "Error receiving candidates",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> UploadUserImageAsync(IFormFile image, UsernameDto? usernameDto)
    {
        try
        {
            var imageResult = await _fileService.UploadUserImageAsync(image);

            if (!imageResult.IsSuccessfully || usernameDto is null)
                return new()
                {
                    Message = "Error uploading image",
                    IsSuccessfully = false
                };

            var user = await GetUserDataAsync(usernameDto);

            if (string.IsNullOrEmpty(user.User.ImagePath))
            {
                user.User.ImagePath = imageResult.FilePath;

                DbSet.Update(user.User);
                await Context.SaveChangesAsync();
            }
            else
            {
                var removeOldUserImage = _fileService.RemoveUserImageAsync(user.User.ImagePath);

                if (!removeOldUserImage.IsSuccessfully)
                    return new()
                    {
                        Message = "Error uploading image while remove old image",
                        IsSuccessfully = false
                    };

                user.User.ImagePath = imageResult.FilePath;
                DbSet.Update(user.User);
                await Context.SaveChangesAsync();
            }

            return new()
            {
                Message = "Image uploaded successfully",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error uploading image");
            return new()
            {
                Message = "Error uploading image",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> RefreshGeneralInfoAsync(RefreshGeneralUserDto? refreshGeneralUserDto)
    {
        try
        {
            if (refreshGeneralUserDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var userResult = await GetUserDataAsync(new() {Username = refreshGeneralUserDto.UserName});
            var user = userResult.User;

            user.FirstName = refreshGeneralUserDto.FirstName;
            user.LastName = refreshGeneralUserDto.LastName;
            user.LinkedInLink = refreshGeneralUserDto.LinkedInLink;
            user.GithubLink = refreshGeneralUserDto.GithubLink;
            user.TelegramLink = refreshGeneralUserDto.TelegramLink;
            user.SkypeLink = refreshGeneralUserDto.SkypeLink;

            DbSet.Update(user);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "User data successfully updated",
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

    public async Task<UserResponse> RefreshCandidateUserInfoAsync(RefreshCandidateDto? refreshCandidateDto)
    {
        try
        {
            if (refreshCandidateDto is null)
                return new()
                {
                    Message = "Error when trying to refresh account",
                    IsSuccessfully = false
                };

            var userResult = await GetUserDataAsync(new()
                {Username = refreshCandidateDto.Username, UserRole = CandidateRole});
            var user = userResult.User;

            if (user.CandidateUser is null)
                return new()
                {
                    Message = "An error occurred while trying to update user data",
                    IsSuccessfully = false
                };

            user.CandidateUser.CompanyPosition = refreshCandidateDto.CompanyPosition;
            user.CandidateUser.ExpectedSalary = refreshCandidateDto.ExpectedSalary;
            user.CandidateUser.HourlyRate = refreshCandidateDto.HourlyRate;
            user.CandidateUser.ExperienceWorkTime = refreshCandidateDto.ExperienceWorkTime;
            user.CandidateUser.ExperienceWorkDescription = refreshCandidateDto.ExperienceWorkDescription;
            user.CandidateUser.Country = refreshCandidateDto.Country;
            user.CandidateUser.City = refreshCandidateDto.City;
            user.CandidateUser.EmploymentOptions = refreshCandidateDto.EmploymentOptions;
            user.CandidateUser.KeyWords = refreshCandidateDto.KeyWords;
            user.CandidateUser.CommunicationLanguageId = refreshCandidateDto.CommunicationLanguageId;
            user.CandidateUser.EnglishLevelId = refreshCandidateDto.EnglishLevelId;
            user.CandidateUser.WorkCategoryId = refreshCandidateDto.WorkCategoryId;
            user.CandidateUser.IsProfileComplete = true;

            DbSet.Update(user);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "Account successfully updated",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when trying to refresh account");
            return new()
            {
                Message = "Error when trying to refresh account",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> RefreshEmployerInfoAsync(RefreshEmployerDto? refreshEmployerDto)
    {
        try
        {
            if (refreshEmployerDto is null)
                return new()
                {
                    Message = "Error when trying to refresh account",
                    IsSuccessfully = false
                };

            var userData = await GetUserDataAsync(new() {Username = refreshEmployerDto.Username});
            var user = userData.User;

            if (user.EmployerUser is null)
                return new()
                {
                    Message = "An error occurred while trying to update user data",
                    IsSuccessfully = false
                };

            user.EmployerUser.CompanyPosition = refreshEmployerDto.CompanyPosition;
            user.EmployerUser.CompanyName = refreshEmployerDto.CompanyName;
            user.EmployerUser.CompanySiteLink = refreshEmployerDto.CompanySiteLink;
            user.EmployerUser.DoyCompanyLink = refreshEmployerDto.DoyCompanyLink;
            user.EmployerUser.AboutCompany = refreshEmployerDto.AboutCompany;

            DbSet.Update(user);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "Account successfully updated",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when trying to refresh account");
            return new()
            {
                Message = "Error when trying to refresh account",
                IsSuccessfully = false
            };
        }
    }

    public async Task<UserResponse> RemoveUserAsync(UsernameDto? usernameDto)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var userResult = await GetUserDataAsync(usernameDto);

            if (!userResult.IsSuccessfully)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            DbSet.Remove(userResult.User);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "User deleted successfully",
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

    public async Task<UserResponse> UpdateCandidateViewNumberIfCountAsync(UsernameDto? usernameDto)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var userResult = await GetUserDataAsync(usernameDto);

            if (!userResult.IsSuccessfully || userResult.User.CandidateUser is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            userResult.User.CandidateUser.ViewCount++;
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "Account views counter updated successfully",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while trying to update the account views counter");
            return new()
            {
                Message = "An error occurred while trying to update the account views counter",
                IsSuccessfully = false
            };
        }
    }
}