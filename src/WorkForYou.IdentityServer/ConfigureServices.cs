using Microsoft.AspNetCore.Identity;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.IdentityServer;

public static class ConfigureServices
{
    public static IServiceCollection AddIdentityServerServices(this IServiceCollection services)
    {
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
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
            options.Cookie.Name = "Identity.Cookie";
        });

        services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources())
            .AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
            .AddInMemoryClients(IdentityServerConfiguration.GetClients())
            .AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential();

        services.AddControllersWithViews();
        
        return services;
    }
}
