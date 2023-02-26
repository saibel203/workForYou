using System.Globalization;
using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using WorkForYou.Core.IOptions;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Data.Repositories;
using WorkForYou.Infrastructure.DatabaseContext;
using WorkForYou.Services;

namespace WorkForYou.WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSignalR();

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

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = new PathString("/auth/login");
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
            options.Cookie.Name = "Identity.Cookie";
            options.AccessDeniedPath = "/Error/AccessDenied";
        });

        services.AddAutoMapper(typeof(Program));

        services.Configure<SendGridOptions>(configuration.GetSection("SendGridOptions"));
        services.Configure<WebUiOptions>(configuration.GetSection("WebUIOptions"));

        services.AddDistributedMemoryCache();
        services.AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(1); });
        
        services.AddHostedService(sp => new NpmWatchHosted(
            enabled: sp.GetRequiredService<IWebHostEnvironment>().IsDevelopment(),
            logger: sp.GetRequiredService<ILogger<NpmWatchHosted>>()));

        services.AddScoped<SeedDbContext>();
        
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IVacancyService, VacancyService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IChatService, ChatService>();

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

        services.AddNotyf(options =>
        {
            options.DurationInSeconds = 10;
            options.IsDismissable = true;
            options.Position = NotyfPosition.TopRight;
        });

        services.AddControllersWithViews()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
            .AddDataAnnotationsLocalization();

        return services;
    }
}