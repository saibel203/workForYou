using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class TypeOfCompanyRepository : GenericRepository<TypesOfCompany>, ITypeOfCompanyRepository
{
    public TypeOfCompanyRepository(WorkForYouDbContext context, ILogger logger) 
        : base(context, logger)
    {
    }

    public async Task<TypesOfCompanyResponse> GetAllTypesOfCompanyAsync()
    {
        try
        {
            var typesOfCompany = await DbSet.ToListAsync();
            
            return new()
            {
                Message = "Data received successfully",
                IsSuccessfully = true,
                TypesOfCompanies = typesOfCompany
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
