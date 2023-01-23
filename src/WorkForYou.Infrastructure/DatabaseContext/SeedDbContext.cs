using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Data.Models;
using WorkForYou.Data.Models.IdentityInheritance;

namespace WorkForYou.Infrastructure.DatabaseContext;

public class SeedDbContext
{
    private readonly WorkForYouDbContext _context;
    private readonly ILogger<SeedDbContext> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public SeedDbContext(WorkForYouDbContext context, ILogger<SeedDbContext> logger, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitialiseDatabaseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
                await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            throw;
        }
    }

    public async Task SeedDataAsync()
    {
        try
        {
            await TrySeedDataAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            throw;
        }
    }

    private async Task TrySeedDataAsync()
    {
        // Seed roles
        var defaultRoles = new ApplicationRole[]
        {
            new("admin", "Адмін має доступ до перегляду всіх акаунтів роботодавців та кандидатів. Він може їх контролювати, попереджати про невідповідності та видаляти їх у критичних ситуаціях."),
            new("candidate", "Робітник (кандидат) може заповнити профіль, прикріпити резюме та відгукуватися на вакансії"),
            new("employer", "Роботодавець може заповнити інформацію про компанію та створювати її вакансії, на які можуть відгукуватися кандидати. Він може спілкуватися з ними, створювати команду, тощо."),
        };

        foreach (var defaultRole in defaultRoles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(defaultRole.Name!);

            if (!roleExist)
                await _roleManager.CreateAsync(defaultRole);
        }
        
        // Seed default supporting data
        var englishLevelsDefault = new EnglishLevel[]
        {
            new() { NameLevel = "No English", DescriptionLevel = "Немає знаннь англійської" },
            new() { NameLevel = "Beginner / Elementary", DescriptionLevel = "Базовий рівень" },
            new() { NameLevel = "Pre-Intermediate", DescriptionLevel = "Може читати технічну документацію, вести базове листування з невеликою допомогою" },
            new() { NameLevel = "Intermediate", DescriptionLevel = "Читає та розмовляє, але простими фразами" },
            new() { NameLevel = "Upper-Intermediate", DescriptionLevel = "Може брати участь у мітингах або проходити співбесіду англійською" },
            new() { NameLevel = "Advanced/Fluent", DescriptionLevel = "Вільна англійська" }
        };
        
        if (await _context.EnglishLevels.CountAsync() == 0)
        {
            await _context.EnglishLevels.AddRangeAsync(englishLevelsDefault);
            await _context.SaveChangesAsync();
        }
        
        var workCategoriesDefault = new WorkCategory[]
        {
            new() { CategoryName = "C# / .NET" },
            new() { CategoryName = "JavaScript / Front-end" },
            new() { CategoryName = "Java" },
            new() { CategoryName = "Python" },
            new() { CategoryName = "PHP" },
            new() { CategoryName = "Node.js" },
            new() { CategoryName = "iOS" },
            new() { CategoryName = "Android" },
            new() { CategoryName = "C / C++ / Embedded" },
            new() { CategoryName = "Ruby" },
            new() { CategoryName = "Golang" },
            new() { CategoryName = "Scala" },
            new() { CategoryName = "Rust" },
            new() { CategoryName = "Flutter" },
            new() { CategoryName = "Gamedev / Unity" },
            new() { CategoryName = "Design / UI/UX" }
        };
        
        if (await _context.WorkCategories.CountAsync() == 0)
        {
            await _context.WorkCategories.AddRangeAsync(workCategoriesDefault);
            await _context.SaveChangesAsync();
        }
        
        // Seed default users
        if (await _context.Users.CountAsync() == 0)
        {
            var defaultUsers = new ApplicationUser[]
            {
                new()
                {
                    FirstName = "Едуард", LastName = "Поповський",
                    UserName = "adminSite", Email = "admin@gmail.com", EmailConfirmed = true
                },
                new()
                {
                    FirstName = "Михайло", LastName = "Довгань",
                    UserName = "candidate", Email = "candidate@gmail.com", EmailConfirmed = true
                },
                new()
                {
                    FirstName = "Анастасія", LastName = "Шарко",
                    UserName = "employer", Email = "employer@gmail.com", EmailConfirmed = true
                }
            };

            foreach (var defaultUser in defaultUsers)
            {
                await _userManager.CreateAsync(defaultUser, "testPassword123");
            }

            await _userManager.AddToRoleAsync(defaultUsers[0], defaultRoles[0].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[1], defaultRoles[1].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[2], defaultRoles[2].Name!);

            var candidateUser = new CandidateUser
            {
                CompanyPosition = ".NET developer",
                ExpectedSalary = 400,
                HourlyRate = 10,
                ExperienceWorkDescription = "I had work experience (1-2 month) in a startup project - development of an analogue of 1C program - mostly engaged in Front-end part and development of controllers with CRUD operations. I did simple MVC projects for example - Blog platform, Simple Shop, Management system, Realty Sale Project. Also several microservice projects for example - Gamer site (ASP.NET core Web API, Angular for client)",
                Country = "Ukraine",
                City = "Rivne",
                LevelEnglish = englishLevelsDefault[2],
                CategoryWork = workCategoriesDefault[0],
                ApplicationUser = defaultUsers[1]
            };
            var employerUser = new EmployerUser
            {
                CompanyName = "GeekForLess",
                CompanySiteLink = "https://geeksforless.com/",
                DoyCompanyLink = "https://jobs.dou.ua/companies/geeksforless/",
                AboutCompany = "",
                ApplicationUser = defaultUsers[2]
            };

            await _context.CandidateUsers.AddAsync(candidateUser);
            await _context.EmployerUsers.AddAsync(employerUser);
            await _context.SaveChangesAsync();
        }
    }
}
