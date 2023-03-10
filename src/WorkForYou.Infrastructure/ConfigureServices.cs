using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var defaultDbConnectionString = configuration.GetConnectionString("DefaultDbConnection");

        services.AddDbContext<WorkForYouDbContext>(options =>
            options.UseSqlServer(defaultDbConnectionString));

        return services;
    }
}