﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class HowToWorkRepository : GenericRepository<HowToWork>, IHowToWorkRepository
{
    public HowToWorkRepository(WorkForYouDbContext context, ILogger logger) 
        : base(context, logger)
    {
    }
    
    public async Task<HowToWorkResponse> GetAllHowToWorkAsync()
    {
        try
        {
            var howToWorkCategories = await DbSet.ToListAsync();

            return new()
            {
                Message = "Data received successfully",
                IsSuccessfully = true,
                HowToWorks = howToWorkCategories
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
