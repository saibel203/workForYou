using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.IdentityModel.Tokens;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Data.Repositories;
using WorkForYou.Services;

namespace WorkForYou.WebAPI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        
        services.AddAutoMapper(typeof(Program));

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
        
        services.AddControllers()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
            .AddDataAnnotationsLocalization();
        
        return services;
    }
}
