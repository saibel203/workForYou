using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.Helpers;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class VacancyRepository : GenericRepository<Vacancy>, IVacancyRepository
{
    private readonly IMapper _mapper;

    private const int PageSize = 7;

    public VacancyRepository(WorkForYouDbContext context, ILogger logger, IMapper mapper)
        : base(context, logger)
    {
        _mapper = mapper;
    }

    public async Task<VacancyResponse> GetAllVacanciesAsync(QueryParameters queryParameters)
    {
        try
        {
            int skipAmount = PageSize * (queryParameters.PageNumber - 1);

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

            vacancies = ListSortingHelper.ListVacancySort(queryParameters, vacancies);
            vacancies = FilteringHelper.VacancyFiltering(queryParameters, vacancies);

            var vacanciesCount = vacancies.Count;
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / PageSize);

            return new()
            {
                Message = "Vacancies successfully received",
                IsSuccessfully = true,
                VacancyList = vacancies
                    .Skip(skipAmount)
                    .Take(PageSize),
                PageCount = pageCount,
                VacancyCount = vacanciesCount,
                SearchString = queryParameters.SearchString,
                SortBy = queryParameters.SortBy,
                WorkCategory = queryParameters.WorkCategory,
                Pages = PaginationHelper.PageNumbers(queryParameters.PageNumber, pageCount)
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

            int skipAmount = PageSize * (queryParameters.PageNumber - 1);

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

            employerVacancies = ListSortingHelper.ListVacancySort(queryParameters, employerVacancies);
            employerVacancies = FilteringHelper.VacancyFiltering(queryParameters, employerVacancies);

            var vacanciesCount = employerVacancies.Count;
            var pageCount = (int) Math.Ceiling((double) vacanciesCount / PageSize);

            return new()
            {
                Message = "The list of employer vacancies has been successfully received",
                IsSuccessfully = true,
                VacancyList = employerVacancies
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
            Logger.LogError(ex, "An error occurred while trying to create a vacancy");

            return new()
            {
                Message = "An error occurred while trying to create a vacancy",
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
            Logger.LogError(ex, "An error occurred while trying to update the job");
            return new()
            {
                Message = "An error occurred while trying to update the job",
                IsSuccessfully = false
            };
        }
    }

    public async Task<VacancyResponse> UpdateViewNumberOfCountAsync(int vacancyId)
    {
        try
        {
            var vacancyResult = await GetVacancyByIdAsync(vacancyId);

            if (!vacancyResult.IsSuccessfully || vacancyResult.Vacancy is null)
                return new()
                {
                    Message = "An error occurred when receiving a vacancy",
                    IsSuccessfully = false
                };

            vacancyResult.Vacancy.ViewCount++;
            await Context.SaveChangesAsync();

            return new()
            {
                Message = "Job view counter has been updated successfully",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while trying to update the job view counter");
            return new()
            {
                Message = "An error occurred while trying to update the job view counter",
                IsSuccessfully = false
            };
        }
    }
}