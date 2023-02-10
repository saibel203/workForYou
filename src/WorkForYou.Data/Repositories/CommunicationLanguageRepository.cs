using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class CommunicationLanguageRepository : GenericRepository<CommunicationLanguage>, ICommunicationLanguageRepository
{
    public CommunicationLanguageRepository(WorkForYouDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public async Task<CommunicationLanguageResponse> GetAllCommunicationLanguagesAsync()
    {
        try
        {
            var communicationLanguageList = await DbSet.ToListAsync();

            return new()
            {
                Message = "Data received successfully",
                IsSuccessfully = true,
                CommunicationLanguages = communicationLanguageList
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
