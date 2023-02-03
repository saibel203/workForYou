using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.DtoModels;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Data.DatabaseContext;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Data.Repositories;

public class VacancyRepository : GenericRepository<Vacancy>, IVacancyRepository
{
    public VacancyRepository(WorkForYouDbContext context, ILogger logger, IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager, IMapper mapper)
        : base(context, logger, httpContextAccessor, userManager, mapper)
    {
    }

    public async Task<VacancyResponse> GetAllVacanciesAsync(int pageNumber, string? searchString)
    {
        try
        {
            const int pageSize = 6;
            int skipAmount = pageSize * (pageNumber - 1);

            IEnumerable<Vacancy> vacancies;

            if (!string.IsNullOrEmpty(searchString))
                vacancies = await DbSet
                    .Include(x => x.EnglishLevel)
                    .Include(x => x.TypeOfCompany)
                    .Include(x => x.CandidateRegion)
                    .Include(x => x.Relocate)
                    .Include(x => x.HowToWork)
                    .Include(x => x.WorkCategory)
                    .Include(x => x.VacancyDomain)
                    .Where(x => EF.Functions.Like(x.VacancyTitle, $"%{searchString}%"))
                    .ToListAsync();
            else
                vacancies = await DbSet
                    .Include(x => x.EnglishLevel)
                    .Include(x => x.TypeOfCompany)
                    .Include(x => x.CandidateRegion)
                    .Include(x => x.Relocate)
                    .Include(x => x.HowToWork)
                    .Include(x => x.WorkCategory)
                    .Include(x => x.VacancyDomain)
                    .ToListAsync();

            int vacanciesCount = vacancies.Count();
            int pageCount = (int) Math.Ceiling((double) vacanciesCount / pageSize);

            return new()
            {
                Message = "Vacancies successfully received",
                IsSuccessfully = true,
                VacancyList = vacancies
                    .Reverse()
                    .Skip(skipAmount)
                    .Take(pageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = searchString,
                Pages = PageNumbers(pageNumber, pageCount)
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Vacancies received error");
            return new()
            {
                Message = "Vacancies received error",
                IsSuccessfully = false,
                VacancyList = new List<Vacancy>()
            };
        }
    }

    public async Task<VacancyResponse> GetAllEmployerVacanciesAsync(UsernameDto? usernameDto, int pageNumber,
        string? searchString)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Data transfer error",
                    IsSuccessfully = false
                };

            const int pageSize = 6;
            int skipAmount = pageSize * (pageNumber - 1);

            var user = await Context.Users
                .Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user is null)
                return new()
                {
                    Message = "The user with the specified Username was not found",
                    IsSuccessfully = false
                };

            var userRole = await UserManager.IsInRoleAsync(user, "employer");

            if (!userRole)
                return new()
                {
                    Message = "The user is not an employer",
                    IsSuccessfully = false
                };

            IEnumerable<Vacancy> employerVacancies;

            if (!string.IsNullOrEmpty(searchString))
                employerVacancies = await DbSet
                    .Include(x => x.EnglishLevel)
                    .Include(x => x.TypeOfCompany)
                    .Include(x => x.CandidateRegion)
                    .Include(x => x.Relocate)
                    .Include(x => x.HowToWork)
                    .Include(x => x.WorkCategory)
                    .Include(x => x.VacancyDomain)
                    .Where(x => x.EmployerUser!.EmployerUserId == user.EmployerUser!.EmployerUserId)
                    .Where(x => EF.Functions.Like(x.VacancyTitle, $"%{searchString}%"))
                    .ToListAsync();
            else
                employerVacancies = await DbSet
                    .Include(x => x.EnglishLevel)
                    .Include(x => x.TypeOfCompany)
                    .Include(x => x.CandidateRegion)
                    .Include(x => x.Relocate)
                    .Include(x => x.HowToWork)
                    .Include(x => x.WorkCategory)
                    .Include(x => x.VacancyDomain)
                    .Where(x => x.EmployerUser!.EmployerUserId == user.EmployerUser!.EmployerUserId)
                    .ToListAsync();

            int vacanciesCount = employerVacancies.Count();
            int pageCount = (int) Math.Ceiling((double) vacanciesCount / pageSize);

            return new()
            {
                Message = "The list of employer vacancies has been successfully received",
                IsSuccessfully = true,
                VacancyList = employerVacancies
                    .Reverse()
                    .Skip(skipAmount)
                    .Take(pageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = searchString,
                Pages = PageNumbers(pageNumber, pageCount)
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Vacancies received error");
            return new()
            {
                Message = "Vacancies received error",
                IsSuccessfully = false,
                VacancyList = new List<Vacancy>()
            };
        }
    }

    public async Task<VacancyResponse> GetVacancyByIdAsync(int vacancyId)
    {
        try
        {
            var vacancy = await DbSet
                .Include(x => x.EmployerUser)
                .Include(x => x.EmployerUser!.ApplicationUser)
                .Include(x => x.WorkCategory)
                .Include(x => x.Relocate)
                .Include(x => x.VacancyDomain)
                .Include(x => x.TypeOfCompany)
                .Include(x => x.CandidateRegion)
                .Include(x => x.EnglishLevel)
                .Include(x => x.HowToWork)
                .FirstOrDefaultAsync(x => x.VacancyId == vacancyId);

            if (vacancy is null)
                return new()
                {
                    Message = "No vacancy with this Id found",
                    IsSuccessfully = false
                };

            if (!HttpContextAccessor.HttpContext.Session.Keys.Contains($"IsShowVacancy{vacancyId}") 
                && HttpContextAccessor.HttpContext.User.IsInRole("candidate"))
            {
                HttpContextAccessor.HttpContext.Session.SetString($"IsShowVacancy{vacancyId}", "1");
                vacancy.ViewCount++;
                await Context.SaveChangesAsync();
            }

            return new()
            {
                Message = "Vacancy successfully receive",
                IsSuccessfully = true,
                Vacancy = vacancy
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred when receiving a vacancy");
            return new()
            {
                Message = "An error occurred when receiving a vacancy",
                IsSuccessfully = false
            };
        }
    }

    public async Task<VacancyResponse> CreateVacancyAsync(ActionVacancyDto? actionVacancyDto)
    {
        try
        {
            if (actionVacancyDto is null)
                return new()
                {
                    Message = "Vacancy model is null",
                    IsSuccessfully = false
                };

            var createVacancyModel = Mapper.Map<Vacancy>(actionVacancyDto);

            await DbSet.AddAsync(createVacancyModel);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "The vacancy has been successfully created",
                IsSuccessfully = true,
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Vacancy model is null");

            return new()
            {
                Message = "Vacancy model is null",
                IsSuccessfully = false
            };
        }
    }

    public async Task<VacancyResponse> RemoveVacancyAsync(int vacancyId)
    {
        try
        {
            var vacancy = await DbSet.FirstOrDefaultAsync(x => x.VacancyId == vacancyId);

            if (vacancy is null)
                return new()
                {
                    Message = "No vacancy with this Id found",
                    IsSuccessfully = false
                };

            DbSet.Remove(vacancy);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "Vacancy successfully remove",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred when remove vacancy");
            return new()
            {
                Message = "An error occurred when remove vacancy",
                IsSuccessfully = false
            };
        }
    }

    public async Task<VacancyResponse> UpdateVacancyAsync(ActionVacancyDto? actionVacancyDto)
    {
        try
        {
            if (actionVacancyDto is null)
                return new()
                {
                    Message = "Vacancy model is null",
                    IsSuccessfully = false
                };

            var vacancy = Mapper.Map<Vacancy>(actionVacancyDto);
            DbSet.Update(vacancy);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "The vacancy has been successfully updated",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Vacancy model is null");
            return new()
            {
                Message = "Vacancy model is null",
                IsSuccessfully = false
            };
        }
    }

    public async Task<VacancyResponse> AddVacancyToFavouriteList(string username, int vacancyId)
    {
        try
        {
            var user = await Context.Users.Include(x => x.CandidateUser)
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (user is null || user.CandidateUser is null)
                return new()
                {
                    Message = "",
                    IsSuccessfully = false
                };

            var vacancy = await DbSet.FirstOrDefaultAsync(x => x.VacancyId == vacancyId);

            if (vacancy is null)
                return new()
                {
                    Message = "",
                    IsSuccessfully = false
                };

            var isElementContainsInList = await Context.FavouriteVacancies
                .AnyAsync(x => x.VacancyId == vacancyId &&
                               x.CandidateId == user.CandidateUser.CandidateUserId);

            var favourite = new FavouriteVacancy
            {
                CandidateId = user.CandidateUser.CandidateUserId,
                VacancyId = vacancyId
            };

            if (isElementContainsInList)
            {
                Context.FavouriteVacancies.Remove(favourite);
                await Context.SaveChangesAsync();

                return new()
                {
                    Message = "",
                    IsSuccessfully = true,
                };
            }

            await Context.FavouriteVacancies.AddAsync(favourite);
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "",
                IsSuccessfully = true,
                FavouriteVacancy = favourite
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