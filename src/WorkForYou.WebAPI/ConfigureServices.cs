using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.IdentityModel.Tokens;
using WorkForYou.Core.IOptions;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Data.Repositories;
using WorkForYou.Infrastructure.DatabaseContext;
using WorkForYou.Services;
using WorkForYou.Shared.Mapping;

namespace WorkForYou.WebAPI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        const string webUiCorsOptions = "WebUICorsPolicy";

        services.AddHttpContextAccessor();

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
            //.AddErrorDescriber<MultiLanguageIdentityErrorDescriber>()
            .AddEntityFrameworkStores<WorkForYouDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var encodingKey = Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!);
                var symmetricSecurityKey = new SymmetricSecurityKey(encodingKey);
                
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidAudience = configuration["JwtOptions:Audience"],
                    IssuerSigningKey = symmetricSecurityKey
                };
            });
        
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddAutoMapper(typeof(AutomapperProfile));

        services.AddTransient<IFavouriteListService, FavouriteListService>();
        services.AddTransient<IApiAuthService, ApiAuthService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            const string defaultCulture = "uk";

            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture), new CultureInfo("en")
            };

            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SetDefaultCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy(webUiCorsOptions, policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddControllers()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
            .AddDataAnnotationsLocalization();

        return services;
    }
}
