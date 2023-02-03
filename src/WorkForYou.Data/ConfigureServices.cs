using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Data.DatabaseContext;
using WorkForYou.Data.Repositories;

namespace WorkForYou.Data;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var defaultDbConnectionString = configuration.GetConnectionString("DefaultDbConnection");

        services.AddDbContext<WorkForYouDbContext>(options =>
            options.UseSqlServer(defaultDbConnectionString));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<WorkForYouDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = new PathString("/auth/login");
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        });

        services.AddScoped<SeedDbContext>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }
}