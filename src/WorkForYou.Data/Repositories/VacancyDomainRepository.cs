using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class VacancyDomainRepository : GenericRepository<VacancyDomain>, IVacancyDomainRepository
{
    public VacancyDomainRepository(WorkForYouDbContext context, ILogger logger, IHttpContextAccessor httpContextAccessor, 
        UserManager<ApplicationUser> userManager, IMapper mapper) 
        : base(context, logger, httpContextAccessor, userManager, mapper)
    {
    }

    public async Task<VacancyDomainResponse> GetAllVacancyDomainsAsync()
    {
        try
        {
            var vacancyDomainsList = await DbSet.ToListAsync();

            return new()
            {
                Message = "Job domains successfully received",
                IsSuccessfully = true,
                VacancyDomains = vacancyDomainsList
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting data");
            return new()
            {
                Message = "Error getting data",
                IsSuccessfully = false
            };
        }
    }
}
