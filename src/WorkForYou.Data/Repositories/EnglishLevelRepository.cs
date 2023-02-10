using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class EnglishLevelRepository : GenericRepository<EnglishLevel>, IEnglishLevelRepository
{
    public EnglishLevelRepository(WorkForYouDbContext context, ILogger logger) 
        : base(context, logger)
    {
    }

    public async Task<EnglishLevelResponse> GetAllEnglishLevelsAsync()
    {
        try
        {
            var englishLevels = await DbSet.ToListAsync();

            return new()
            {
                Message = "English levels successfully obtained",
                IsSuccessfully = true,
                EnglishLevels = englishLevels
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while receiving data");
            return new()
            {
                Message = "An error occurred while receiving data",
                IsSuccessfully = false
            };
        }
    }
}
