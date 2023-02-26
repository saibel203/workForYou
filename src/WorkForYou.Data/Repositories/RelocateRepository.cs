using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class RelocateRepository : GenericRepository<Relocate>, IRelocateRepository
{
    public RelocateRepository(WorkForYouDbContext context, ILogger logger) 
        : base(context, logger)
    {
    }

    public async Task<RelocateResponse> GetAllRelocatesAsync()
    {
        try
        {
            var relocates = await DbSet.ToListAsync();

            return new()
            {
                Message = "Data received successfully",
                IsSuccessfully = true,
                Relocates = relocates
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
