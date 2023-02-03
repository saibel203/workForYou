using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using WorkForYou.Core.IOptions;
using WorkForYou.Core.IServices;
using WorkForYou.Services;

namespace WorkForYou.WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Program));

        services.Configure<SendGridOptions>(configuration.GetSection("SendGridOptions"));
        services.Configure<WebUiOptions>(configuration.GetSection("WebUIOptions"));

        services.AddDistributedMemoryCache();
        services.AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(1); });


        services.AddHostedService(sp => new NpmWatchHosted(
            enabled: sp.GetRequiredService<IWebHostEnvironment>().IsDevelopment(),
            logger: sp.GetRequiredService<ILogger<NpmWatchHosted>>()));

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IMailService, MailService>();

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

        services.AddControllersWithViews()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
            .AddDataAnnotationsLocalization();

        return services;
    }
}