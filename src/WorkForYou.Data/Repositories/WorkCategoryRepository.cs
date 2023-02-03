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

public class WorkCategoryRepository : GenericRepository<WorkCategory>, IWorkCategoryRepository
{
    public WorkCategoryRepository(WorkForYouDbContext context, ILogger logger, IHttpContextAccessor httpContextAccessor, 
        UserManager<ApplicationUser> userManager, IMapper mapper) 
        : base(context, logger, httpContextAccessor, userManager, mapper)
    {
    }

    public async Task<WorkCategoryResponse> GetAllWorkCategoriesAsync()
    {
        try
        {
            var workCategories = await DbSet.ToListAsync();

            return new()
            {
                Message = "Categories received successfully",
                IsSuccessfully = true,
                WorkCategories = workCategories
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting categories");
            return new()
            {
                Message = "Error getting categories",
                IsSuccessfully = false
            };
        }
    }
}
