using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.Helpers;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class VacancyRepository : GenericRepository<Vacancy>, IVacancyRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public VacancyRepository(WorkForYouDbContext context, ILogger logger, IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
        : base(context, logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<VacancyResponse> GetAllVacanciesAsync(QueryParameters queryParameters)
    {
        try
        {
            const int pageSize = 6;
            int skipAmount = pageSize * (queryParameters.PageNumber - 1);

            IReadOnlyList<Vacancy> vacancies;

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                vacancies = await DbSet
                    .Include(x => x.EnglishLevel)
                    .Include(x => x.TypeOfCompany)
                    .Include(x => x.CandidateRegion)
                    .Include(x => x.Relocate)
                    .Include(x => x.HowToWork)
                    .Include(x => x.WorkCategory)
                    .Include(x => x.VacancyDomain)
                    .Include(x => x.EmployerUser)
                    .Include(x => x.EmployerUser!.ApplicationUser)
                    .Where(x => EF.Functions.Like(x.VacancyTitle, $"%{queryParameters.SearchString}%"))
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
                    .Include(x => x.EmployerUser)
                    .Include(x => x.EmployerUser!.ApplicationUser)
                    .ToListAsync();

            if (queryParameters.SortBy is not null)
                vacancies = ListSortingHelper.ListVacancySort(queryParameters.SortBy, vacancies);

            var vacanciesCount = vacancies.Count;
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / pageSize);

            return new()
            {
                Message = "Vacancies successfully received",
                IsSuccessfully = true,
                VacancyList = vacancies
                    .Skip(skipAmount)
                    .Take(pageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = queryParameters.SearchString,
                Pages = PaginationHelper.PageNumbers(queryParameters.PageNumber, pageCount),
                SortBy = queryParameters.SortBy
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

    public async Task<VacancyResponse> GetAllEmployerVacanciesAsync(UsernameDto? usernameDto,
        QueryParameters queryParameters)
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
            int skipAmount = pageSize * (queryParameters.PageNumber - 1);

            var user = await Context.Users
                .Include(x => x.EmployerUser)
                .FirstOrDefaultAsync(x => x.UserName == usernameDto.Username);

            if (user is null)
                return new()
                {
                    Message = "The user with the specified Username was not found",
                    IsSuccessfully = false
                };

            IReadOnlyList<Vacancy> employerVacancies;

            if (!string.IsNullOrEmpty(queryParameters.SearchString))
                employerVacancies = await DbSet
                    .Include(x => x.EnglishLevel)
                    .Include(x => x.TypeOfCompany)
                    .Include(x => x.CandidateRegion)
                    .Include(x => x.Relocate)
                    .Include(x => x.HowToWork)
                    .Include(x => x.WorkCategory)
                    .Include(x => x.VacancyDomain)
                    .Where(x => x.EmployerUser!.EmployerUserId == user.EmployerUser!.EmployerUserId)
                    .Where(x => EF.Functions.Like(x.VacancyTitle, $"%{queryParameters.SearchString}%"))
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

            if (queryParameters.SortBy is not null)
                employerVacancies = ListSortingHelper.ListVacancySort(queryParameters.SortBy, employerVacancies);

            var vacanciesCount = employerVacancies.Count;
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / pageSize);

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
                SearchString = queryParameters.SearchString,
                Pages = PaginationHelper.PageNumbers(queryParameters.PageNumber, pageCount),
                SortBy = queryParameters.SortBy
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
                    .ThenInclude(x => x!.ApplicationUser)
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

            const string candidateRole = "candidate";

            if (!_httpContextAccessor.HttpContext.Session.Keys.Contains($"IsShowVacancy{vacancyId}")
                && _httpContextAccessor.HttpContext.User.IsInRole(candidateRole))
            {
                _httpContextAccessor.HttpContext.Session.SetString($"IsShowVacancy{vacancyId}", "1");
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

            var createVacancyModel = _mapper.Map<Vacancy>(actionVacancyDto);

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

            var vacancy = _mapper.Map<Vacancy>(actionVacancyDto);
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
}