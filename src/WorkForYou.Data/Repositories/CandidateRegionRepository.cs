using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.Models;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class CandidateRegionRepository : GenericRepository<CandidateRegion>, ICandidateRegionRepository
{
    public CandidateRegionRepository(WorkForYouDbContext context, ILogger logger) 
        : base(context, logger)
    {
    }

    public async Task<CandidateRegionResponse> GetAllCandidateRegionsAsync()
    {
        try
        {
            var candidateRegions = await DbSet.ToListAsync();

            return new()
            {
                Message = "Candidate regions successfully obtained",
                IsSuccessfully = true,
                CandidateRegions = candidateRegions
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
