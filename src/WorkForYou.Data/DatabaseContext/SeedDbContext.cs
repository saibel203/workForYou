using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Data.DatabaseContext;

public class SeedDbContext
{
    private readonly WorkForYouDbContext _context;
    private readonly ILogger<SeedDbContext> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public SeedDbContext(WorkForYouDbContext context, ILogger<SeedDbContext> logger,
        RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
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
            new("admin",
                "Адмін має доступ до перегляду всіх акаунтів роботодавців та кандидатів. Він може їх контролювати, попереджати про невідповідності та видаляти їх у критичних ситуаціях."),
            new("candidate",
                "Робітник (кандидат) може заповнити профіль, прикріпити резюме та відгукуватися на вакансії"),
            new("employer",
                "Роботодавець може заповнити інформацію про компанію та створювати її вакансії, на які можуть відгукуватися кандидати. Він може спілкуватися з ними, створювати команду, тощо."),
        };

        foreach (var defaultRole in defaultRoles)
        {
            var roleExist = await _roleManager.RoleExistsAsync(defaultRole.Name!);

            if (!roleExist)
                await _roleManager.CreateAsync(defaultRole);
        }

        // Seed communication languages data
        var communicationLanguagesData = new CommunicationLanguage[]
        {
            new() {CommunicationLanguageName = "Українська"},
            new() {CommunicationLanguageName = "Англійська"}
        };

        if (await _context.CommunicationLanguages.CountAsync() == 0)
        {
            await _context.CommunicationLanguages.AddRangeAsync(communicationLanguagesData);
            await _context.SaveChangesAsync();
        }

        // Seed english level data
        var englishLevelsDefault = new EnglishLevel[]
        {
            new() {NameLevel = "No English", DescriptionLevel = "Немає знаннь англійської"},
            new() {NameLevel = "Beginner / Elementary", DescriptionLevel = "Базовий рівень"},
            new()
            {
                NameLevel = "Pre-Intermediate",
                DescriptionLevel = "Може читати технічну документацію, вести базове листування з невеликою допомогою"
            },
            new() {NameLevel = "Intermediate", DescriptionLevel = "Читає та розмовляє, але простими фразами"},
            new()
            {
                NameLevel = "Upper-Intermediate",
                DescriptionLevel = "Може брати участь у мітингах або проходити співбесіду англійською"
            },
            new() {NameLevel = "Advanced/Fluent", DescriptionLevel = "Вільна англійська"}
        };

        if (await _context.EnglishLevels.CountAsync() == 0)
        {
            await _context.EnglishLevels.AddRangeAsync(englishLevelsDefault);
            await _context.SaveChangesAsync();
        }

        // Seed work categories data
        var workCategoriesDefault = new WorkCategory[]
        {
            new() {CategoryName = "C# / .NET"},
            new() {CategoryName = "JavaScript / Front-end"},
            new() {CategoryName = "Java"},
            new() {CategoryName = "Python"},
            new() {CategoryName = "PHP"},
            new() {CategoryName = "Node.js"},
            new() {CategoryName = "iOS"},
            new() {CategoryName = "Android"},
            new() {CategoryName = "C / C++ / Embedded"},
            new() {CategoryName = "Ruby"},
            new() {CategoryName = "Golang"},
            new() {CategoryName = "Scala"},
            new() {CategoryName = "Rust"},
            new() {CategoryName = "Flutter"},
            new() {CategoryName = "Salesforce"},
            new() {CategoryName = "QA Manual"},
            new() {CategoryName = "QA Automation"},
            new() {CategoryName = "Design / UI/UX"},
            new() {CategoryName = "2D/3D Artist / Illustrator"},
            new() {CategoryName = "Project Manager"},
            new() {CategoryName = "Product Manager"},
            new() {CategoryName = "Architect / CTO"},
            new() {CategoryName = "DevOps"},
            new() {CategoryName = "Business Analyst"},
            new() {CategoryName = "Data Science"},
            new() {CategoryName = "Data Analyst"},
            new() {CategoryName = "Sysadmin"},
            new() {CategoryName = "Gamedev / Unity"},
            new() {CategoryName = "Gamedev / Unreal Engine / C++"},
            new() {CategoryName = "SQL / DBA"},
            new() {CategoryName = "Security"},
            new() {CategoryName = "Data Engineer"},
            new() {CategoryName = "Scrum Master / Agile Coach"},
            new() {CategoryName = "Marketing"},
            new() {CategoryName = "HR"},
            new() {CategoryName = "Recruiter"},
            new() {CategoryName = "Customer/Technical Support"},
            new() {CategoryName = "Sales"},
            new() {CategoryName = "SEO"},
            new() {CategoryName = "Technical Writing"},
            new() {CategoryName = "Lead Generation"}
        };

        if (await _context.WorkCategories.CountAsync() == 0)
        {
            await _context.WorkCategories.AddRangeAsync(workCategoriesDefault);
            await _context.SaveChangesAsync();
        }

        // Seed HowToWork data
        var howToWorkDefault = new HowToWork[]
        {
            new() {HowToWorkName = "Тільки офіс"},
            new() {HowToWorkName = "Тільки віддалено"},
            new() {HowToWorkName = "Гібридна робота"},
            new() {HowToWorkName = "На вибір кандидата"}
        };

        if (await _context.HowToWorks.CountAsync() == 0)
        {
            await _context.HowToWorks.AddRangeAsync(howToWorkDefault);
            await _context.SaveChangesAsync();
        }

        // Seed Relocate data
        var relocateDefault = new Relocate[]
        {
            new() {RelocateName = "Без"},
            new() {RelocateName = "За рахунок кандидата"},
            new() {RelocateName = "За рахунок компанії"}
        };

        if (await _context.Relocates.CountAsync() == 0)
        {
            await _context.Relocates.AddRangeAsync(relocateDefault);
            await _context.SaveChangesAsync();
        }

        // Seed TypeOfCompany data
        var typeOfCompanyDefault = new TypesOfCompany[]
        {
            new() {TypeOfCompanyName = "Product"},
            new() {TypeOfCompanyName = "Outsource"},
            new() {TypeOfCompanyName = "Outstaff"},
            new() {TypeOfCompanyName = "Агенція"}
        };

        if (await _context.TypesOfCompany.CountAsync() == 0)
        {
            await _context.TypesOfCompany.AddRangeAsync(typeOfCompanyDefault);
            await _context.SaveChangesAsync();
        }

        // Seed VacancyDomain data
        var vacancyDomainDefault = new VacancyDomain[]
        {
            new() {VacancyDomainName = "Adult"},
            new() {VacancyDomainName = "Advertising / Marketing"},
            new() {VacancyDomainName = "Automotive"},
            new() {VacancyDomainName = "Blockchain / Crypto"},
            new() {VacancyDomainName = "Dating"},
            new() {VacancyDomainName = "E-commerce / Marketplace"},
            new() {VacancyDomainName = "Education"},
            new() {VacancyDomainName = "Fintech"},
            new() {VacancyDomainName = "Gambling"},
            new() {VacancyDomainName = "Gamedev"},
            new() {VacancyDomainName = "Hardware / IoT"},
            new() {VacancyDomainName = "Healthcare / MedTech"},
            new() {VacancyDomainName = "Machine Learning / Big Data"},
            new() {VacancyDomainName = "Media"},
            new() {VacancyDomainName = "Mobile"},
            new() {VacancyDomainName = "SaaS"},
            new() {VacancyDomainName = "Security"},
            new() {VacancyDomainName = "Telecom / Communications"},
            new() {VacancyDomainName = "Travel / Tourism"}
        };

        if (await _context.VacancyDomains.CountAsync() == 0)
        {
            await _context.VacancyDomains.AddRangeAsync(vacancyDomainDefault);
            await _context.SaveChangesAsync();
        }

        // Seed candidate regions data
        var candidateRegionsDefault = new CandidateRegion[]
        {
            new() {CandidateRegionName = "Весь світ"},
            new() {CandidateRegionName = "Україна + Європа"},
            new() {CandidateRegionName = "Лише Європа"},
            new() {CandidateRegionName = "Лише Україна"}
        };

        if (await _context.CandidateRegions.CountAsync() == 0)
        {
            await _context.CandidateRegions.AddRangeAsync(candidateRegionsDefault);
            await _context.SaveChangesAsync();
        }

        // Seed default users
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

        var candidateUser = new CandidateUser
        {
            CompanyPosition = ".NET developer",
            ExpectedSalary = 400,
            HourlyRate = 10,
            ExperienceWorkDescription =
                "I had work experience (1-2 month) in a startup project - development of an analogue of 1C program - mostly engaged in Front-end part and development of controllers with CRUD operations. I did simple MVC projects for example - Blog platform, Simple Shop, Management system, Realty Sale Project. Also several microservice projects for example - Gamer site (ASP.NET core Web API, Angular for client)",
            Country = "Ukraine",
            City = "Rivne",
            LevelEnglish = englishLevelsDefault[2],
            CategoryWork = workCategoriesDefault[0],
            ApplicationUser = defaultUsers[1],
            CommunicationLanguage = communicationLanguagesData[0]
        };
        var employerUser = new EmployerUser
        {
            CompanyName = "GeekForLess",
            CompanyPosition = "HR",
            CompanySiteLink = "https://geeksforless.com/",
            DoyCompanyLink = "https://jobs.dou.ua/companies/geeksforless/",
            AboutCompany =
                "Компанія АЛЛО – маркетплейс національного масштабу з товарами від косметики й інструментів до одягу й меблів та доставкою замовлень на пошту, додому та до магазинів у 140 містах України. Allo.ua – тицяй що хочеш!",
            ApplicationUser = defaultUsers[2]
        };

        if (await _context.Users.CountAsync() == 0)
        {
            foreach (var defaultUser in defaultUsers)
            {
                string userPassword = "testPassword123";
                await _userManager.CreateAsync(defaultUser, userPassword);
            }

            await _userManager.AddToRoleAsync(defaultUsers[0], defaultRoles[0].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[1], defaultRoles[1].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[2], defaultRoles[2].Name!);

            await _context.CandidateUsers.AddAsync(candidateUser);
            await _context.EmployerUsers.AddAsync(employerUser);
            await _context.SaveChangesAsync();
        }

        // Seed default vacancies
        var vacanciesDefault = new Vacancy[]
        {
            new()
            {
                VacancyTitle = "Trainee C# Developer",
                ShortDescription = "DCT team looking for Trainee C# Developer!",
                LongDescription =
                    "<p>Requirements:- good knowledge of C#;</p><p></p><ul><li>good knowledge of C#;</li><li>good understanding of OOP;</li><li>understanding of SQL basics;</li><li>desire and ability to learn, target oriented</li></ul><br><p></p><p>Would be a plus:<br></p><p></p><ul><li>knowledge of design patterns, SOLID;<br></li><li>knowledge of WPF, UWP;<br></li><li>being a quick learner and have desire to improve technical skills;<br></li><li>1+ years of software development experience;<br></li></ul><p></p>",
                VacancyDomain = vacancyDomainDefault[3],
                WorkCategory = workCategoriesDefault[3],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[1],
                EnglishLevel = englishLevelsDefault[3],
                TypeOfCompany = typeOfCompanyDefault[1],
                EmployerUser = employerUser,
                FromSalary = 1200,
                ToSalary = 2000,
                ExperienceWork = 3
            },
            new()
            {
                VacancyTitle = "Marketing Specialist",
                ShortDescription =
                    "You're a fearless marketing enthusiast who loves generating bold ideas, making magic with conversion growth, and managing a marketing strategy? Well, Redwerk has the job just for you!",
                LongDescription =
                    "<p>Вітаємо!</p><p><br></p><p>Ми активно шукаємо Junior Back-End .NET розробника, який долучиться до нашої команди.</p><p><br></p><p>Про проект: Ігровий проект GTA5 на Rage MP (Це платформа, яка зв'язує сервер з клієнтом)</p><p><br></p><p>На одному сервері одночасно грають 1000-1500 людей, по-суті це є классичним лайф-сімом, тому кількість важких систем дуже висока.</p><p><br></p><p>В команді вже є два Back-End розробника, які з радістю будуть ділитися своїм досвідом, та допомогати швидве влитися до команди.</p><p><br></p><p>Пріорітет - Full-Time, можливо Part-time</p><p><br></p><p>Необхідно буде виконати тестове завдання, після успішного проходження загального і технічного інтерв'ю.</p><p>Наявність кейсів та досвіду можуть виключити цей пункт.</p><p><br></p><p><br></p><p>Обов'язки:</p><p>- Підтримка та покращения існуючого коду.</p><p>- Розробка геймплейних фічей на С#</p><p>- Код рев'ю та рефакторінг</p><p>- Спілкування та взаємодія з різними командами (Гейм-Дизайнери, Дизайнери, 3Д спеціалісти)</p><p><br></p><p>Вимоги:</p><p>- Бажання розвиватись профессійно, та бажання делівірети якісний результат для гравців. Розуміння спеціфіки Gaming розробки</p><p>- Client-Server application understanding</p><p>- OOP</p><p>- C#</p><p>- Git</p><p>- SQL</p><p><br></p><p>Буде перевагою:</p><p>- HTML</p><p>- CSS</p><p>- JS</p><p>- Windows Server</p><p>- MySQL</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[5],
                WorkCategory = workCategoriesDefault[5],
                HowToWork = howToWorkDefault[3],
                Relocate = relocateDefault[0],
                CandidateRegion = candidateRegionsDefault[3],
                EnglishLevel = englishLevelsDefault[1],
                TypeOfCompany = typeOfCompanyDefault[2],
                EmployerUser = employerUser,
                FromSalary = 200,
                ToSalary = 500,
                ExperienceWork = 2
            }
        };

        if (await _context.Vacancies.CountAsync() == 0)
        {
            await _context.Vacancies.AddRangeAsync(vacanciesDefault);
            await _context.SaveChangesAsync();
        }

        // Seed FavouriteList data
        var favouriteListDefault = new FavouriteVacancy[]
        {
            new() {CandidateUser = candidateUser, Vacancy = vacanciesDefault[0]}
        };

        if (await _context.FavouriteVacancies.CountAsync() == 0)
        {
            await _context.FavouriteVacancies.AddRangeAsync(favouriteListDefault);
            await _context.SaveChangesAsync();
        }
    }
}