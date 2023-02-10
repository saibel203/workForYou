using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.IServices;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFileService _fileService;
    private readonly IAuthService _authService;

    public UserRepository(WorkForYouDbContext context, ILogger logger, IHttpContextAccessor httpContextAccessor,
        IFileService fileService, IAuthService authService)
        : base(context, logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _fileService = fileService;
        _authService = authService;
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

            var userRole = await _authService.IsUserCandidate(usernameDto);

            if (!userRole.IsSuccessfully)
                return new()
                {
                    Message = "Error defining user role",
                    IsSuccessfully = false
                };

            if (userRole.IsUserCandidate)
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

                if (!_httpContextAccessor.HttpContext.Session.Keys.Contains(
                        $"IsShowVacancy{candidateUser.CandidateUser!.CandidateUserId}")
                    && _httpContextAccessor.HttpContext.User.IsInRole("employer"))
                {
                    _httpContextAccessor.HttpContext.Session.SetString(
                        $"IsShowVacancy{candidateUser.CandidateUser!.CandidateUserId}", "1");
                    candidateUser.CandidateUser!.ViewCount++;
                    await Context.SaveChangesAsync();
                }

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
                FavouriteList = favouriteProperties!
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

    public async Task<UserResponse> GetAllCandidatesAsync(QueryParameters queryParameters)
    {
        try
        {
            const int pageSize = 6;
            int skipAmount = pageSize * (queryParameters.PageNumber - 1);

            IEnumerable<ApplicationUser> users;

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                users = await DbSet
                    .Where(x => x.CandidateUser != null && x.CandidateUser.IsProfileComplete)
                    .Include(x => x.CandidateUser!.CategoryWork)
                    .Include(x => x.CandidateUser!.LevelEnglish)
                    .Include(x => x.CandidateUser!.CommunicationLanguage)
                    .Where(x => EF.Functions.Like(x.CandidateUser!.CompanyPosition!, $"%{queryParameters.SearchString}%")
                                || EF.Functions.Like(x.CandidateUser!.KeyWords!, $"%{queryParameters.SearchString}%"))
                    .ToListAsync();
            else
                users = await DbSet
                    .Where(x => x.CandidateUser != null && x.CandidateUser.IsProfileComplete)
                    .Include(x => x.CandidateUser!.CategoryWork)
                    .Include(x => x.CandidateUser!.LevelEnglish)
                    .Include(x => x.CandidateUser!.CommunicationLanguage)
                    .ToListAsync();
            
            switch (queryParameters.SortBy)
            {
                case "publication-date":
                    users = users.OrderByDescending(x => x.CandidateUser!.CreatedDate).ToList();
                    break;
                case "from-salary":
                    users = users.OrderBy(x => x.CandidateUser!.ExpectedSalary).ToList();
                    break;
                case "to-salary":
                    users = users.OrderByDescending(x => x.CandidateUser!.ExpectedSalary).ToList();
                    break;
                case "from-experience":
                    users = users.OrderBy(x => x.CandidateUser!.ExperienceWorkTime).ToList();
                    break;
                case "to-experience":
                    users = users.OrderByDescending(x => x.CandidateUser!.ExperienceWorkTime).ToList();
                    break;
                case "from-view-count":
                    users = users.OrderBy(x => x.CandidateUser!.ViewCount).ToList();
                    break;
                case "to-view-count":
                    users = users.OrderByDescending(x => x.CandidateUser!.ViewCount).ToList();
                    break;
                // case "from-reviews-count":
                //     vacancies = vacancies.OrderBy(x => x.FromSalary).ToList();
                //     break;
                // case "to-reviews-count":
                //     vacancies = vacancies.OrderByDescending(x => x.FromSalary).ToList();
                //     break;
                default:
                    users = users.OrderBy(x => x.CandidateUser!.CreatedDate).ToList();
                    break;
            }

            int vacanciesCount = users.Count();
            int pageCount = (int) Math.Ceiling((double) vacanciesCount / pageSize);

            return new()
            {
                Message = "Candidates successfully received",
                IsSuccessfully = true,
                ApplicationUsers = users
                    .Reverse()
                    .Skip(skipAmount)
                    .Take(pageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = queryParameters.SearchString,
                SortBy = queryParameters.SortBy,
                Pages = PageNumbers(queryParameters.PageNumber, pageCount)
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

    public async Task<UserResponse> UploadUserImageAsync(IFormFile image, UsernameDto? usernameDto)
    {
        try
        {
            var imageResult = await _fileService.UploadUserImageAsync(image);

            if (!imageResult.IsSuccessfully)
                return new()
                {
                    Message = "Error uploading image",
                    IsSuccessfully = false
                };

            if (usernameDto is null)
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

            var userResult = await GetUserDataAsync(new() {Username = refreshCandidateDto.Username});
            var user = userResult.User;
            user.CandidateUser!.CompanyPosition = refreshCandidateDto.CompanyPosition;
            user.CandidateUser!.ExpectedSalary = refreshCandidateDto.ExpectedSalary;
            user.CandidateUser!.HourlyRate = refreshCandidateDto.HourlyRate;
            user.CandidateUser!.ExperienceWorkTime = refreshCandidateDto.ExperienceWorkTime;
            user.CandidateUser!.ExperienceWorkDescription = refreshCandidateDto.ExperienceWorkDescription;
            user.CandidateUser!.Country = refreshCandidateDto.Country;
            user.CandidateUser!.City = refreshCandidateDto.City;
            user.CandidateUser!.EmploymentOptions = refreshCandidateDto.EmploymentOptions;
            user.CandidateUser!.KeyWords = refreshCandidateDto.KeyWords;
            user.CandidateUser!.CommunicationLanguageId = refreshCandidateDto.CommunicationLanguageId;
            user.CandidateUser!.EnglishLevelId = refreshCandidateDto.EnglishLevelId;
            user.CandidateUser!.WorkCategoryId = refreshCandidateDto.WorkCategoryId;
            user.CandidateUser!.IsProfileComplete = true;

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

            user.EmployerUser!.CompanyPosition = refreshEmployerDto.CompanyPosition;
            user.EmployerUser!.CompanyName = refreshEmployerDto.CompanyName;
            user.EmployerUser!.CompanySiteLink = refreshEmployerDto.CompanySiteLink;
            user.EmployerUser!.DoyCompanyLink = refreshEmployerDto.DoyCompanyLink;
            user.EmployerUser!.AboutCompany = refreshEmployerDto.AboutCompany;

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

    private IEnumerable<int> PageNumbers(int pageNumber, int pageCount)
    {
        if (pageCount <= 5)
        {
            for (int i = 1; i <= pageCount; i++)
            {
                yield return i;
            }
        }
        else
        {
            int midPoint = pageNumber < 3 ? 3
                : pageNumber > pageCount - 2 ? pageCount - 2
                : pageNumber;

            int lowerBound = midPoint - 2;
            int upperBound = midPoint + 2;

            if (lowerBound != 1)
            {
                yield return 1;
                if (lowerBound - 1 > 1)
                    yield return -1;
            }

            for (int i = midPoint - 2; i <= upperBound; i++)
                yield return i;


            if (upperBound != pageCount)
            {
                if (pageCount - upperBound > 1)
                    yield return -1;


                yield return pageCount;
            }
        }
    }
}