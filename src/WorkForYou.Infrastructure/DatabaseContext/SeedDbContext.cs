using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Infrastructure.DatabaseContext;

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
            _logger.LogError(ex, "An error occurred while trying to initialize the database");
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
            _logger.LogError(ex, "An error occurred while trying to seed the database");
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

        if (!await _context.CommunicationLanguages.AnyAsync())
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

        if (!await _context.EnglishLevels.AnyAsync())
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

        if (!await _context.WorkCategories.AnyAsync())
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

        if (!await _context.HowToWorks.AnyAsync())
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

        if (!await _context.Relocates.AnyAsync())
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

        if (!await _context.TypesOfCompany.AnyAsync())
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

        if (!await _context.VacancyDomains.AnyAsync())
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

        if (!await _context.CandidateRegions.AnyAsync())
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
            },
            new()
            {
                FirstName = "Василь", LastName = "Михайлов",
                UserName = "employer2", Email = "employer2@gmail.com", EmailConfirmed = true
            },
            new()
            {
                FirstName = "Еммануїл", LastName = "Стефанів",
                UserName = "candidate2", Email = "candidate2@gmail.com", EmailConfirmed = true
            },
            new()
            {
                FirstName = "Йосип", LastName = "Дем'янюк",
                UserName = "candidate3", Email = "candidate3@gmail.com", EmailConfirmed = true
            },
            new()
            {
                FirstName = "Ліщинський", LastName = "Олег",
                UserName = "candidate4", Email = "candidate4@gmail.com", EmailConfirmed = true
            },
            new()
            {
                FirstName = "Кривенко", LastName = "Гліб",
                UserName = "candidate5", Email = "candidate5@gmail.com", EmailConfirmed = true
            },
            new()
            {
                FirstName = "Підгорний", LastName = "Азар",
                UserName = "candidate6", Email = "candidate6@gmail.com", EmailConfirmed = true
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
            City = "Kyiv",
            LevelEnglish = englishLevelsDefault[2],
            CategoryWork = workCategoriesDefault[0],
            ApplicationUser = defaultUsers[1],
            CommunicationLanguage = communicationLanguagesData[0],
            IsProfileComplete = true,
            KeyWords = 
                ".NET, C#, Asp.Net Core, Web API, .NET developer, EntityFrameworkCore, .ADO NET, HTML, CSS, JS"
        };
        var candidateUser2 = new CandidateUser
        {
            CompanyPosition = "Front-end developer",
            ExpectedSalary = 1000,
            ExperienceWorkTime = 2,
            HourlyRate = 20,
            ExperienceWorkDescription = 
                "Yigzu e-commerce app. Yigzu is an eCommerce app that has multiple functionalities and has two part client part and a drives part I have multiple libraries like Google map, geolocator, stream functionality, pusher for notification, etc. Ethio saq be saq",
            Country = "Ukraine",
            City = "Herson",
            LevelEnglish = englishLevelsDefault[1],
            CategoryWork = workCategoriesDefault[1],
            ApplicationUser = defaultUsers[4],
            CommunicationLanguage = communicationLanguagesData[0],
            IsProfileComplete = true,
            KeyWords = "Front-end, Front-end developer, Angular, Typescript, JavaScript, JS, HTML, CSS"
        };
        var candidateUser3 = new CandidateUser
        {
            CompanyPosition = "DevOps engineer",
            ExpectedSalary = 2000,
            ExperienceWorkTime = 3, 
            HourlyRate = 20,
            ExperienceWorkDescription = 
                "As a DevOps Engineer and former experienced Full-Stack Engineer, I bring a unique combination of skills to the table. I have a proven track record of creating stable, secure, and highly performant web applications. Additionally, I am well-versed in implementing DevOps strategies, building infrastructure as code, creating efficient delivery pipelines, and managing deployment automation and release management effectively. My expertise in both development and operations makes me a valuable asset for any organization. I also have experience in administering Linux systems, which enables me to efficiently and effectively manage and maintain the underlying infrastructure of the applications I work on. I am also highly skilled in identifying and resolving a wide range of bugs and issues, both in application code and in the underlying infrastructure, ensuring minimal disruption to the overall performance and stability of the application",
            Country = "Ukraine",
            City = "Kyiv",
            LevelEnglish = englishLevelsDefault[4],
            CategoryWork = workCategoriesDefault[22],
            ApplicationUser = defaultUsers[5],
            CommunicationLanguage = communicationLanguagesData[1],
            IsProfileComplete = true,
            KeyWords =
                "C#, .NET, ASP.NET core, ASP.NET core MVC, ASP.NET core WEB API, EntityFrameworkCore, ADO.NET, Patterns, LINQ, MsSQL, T-SQL, HTML, CSS, JS, Angular, Typescript, Data Structures and Algorithms"
        };
        var candidateUser4 = new CandidateUser
        {
            CompanyPosition = "PHP Developer",
            ExpectedSalary = 1000,
            ExperienceWorkTime = 1,
            HourlyRate = 30,
            ExperienceWorkDescription = 
                "Worked with different projects. Worked with WordPress, first 1.5 year, it`s system install & configuration, theme design & development.",
            Country = "Ukraine",
            City = "Lviv",
            LevelEnglish = englishLevelsDefault[3],
            CategoryWork = workCategoriesDefault[4],
            ApplicationUser = defaultUsers[6],
            CommunicationLanguage = communicationLanguagesData[1],
            IsProfileComplete = true,
            KeyWords =
                "C#, .NET, ASP.NET core, ASP.NET core MVC, ASP.NET core WEB API, EntityFrameworkCore, ADO.NET, Patterns, LINQ, MsSQL, T-SQL, HTML, CSS, JS, Angular, Typescript, Data Structures and Algorithms"
        };
        var candidateUser5 = new CandidateUser
        {
            CompanyPosition = "Senior Java Engineer",
            ExpectedSalary = 4000,
            ExperienceWorkTime = 5,
            HourlyRate = 100,
            ExperienceWorkDescription = 
                "Senior java engineer, experience over 10 years. Worked in such areas: fin-tech, start-up's, e-commerce.",
            Country = "Ukraine",
            City = "Odessa",
            LevelEnglish = englishLevelsDefault[2],
            CategoryWork = workCategoriesDefault[2],
            ApplicationUser = defaultUsers[7],
            CommunicationLanguage = communicationLanguagesData[1],
            IsProfileComplete = true,
            KeyWords =
                "C#, .NET, ASP.NET core, ASP.NET core MVC, ASP.NET core WEB API, EntityFrameworkCore, ADO.NET, Patterns, LINQ, MsSQL, T-SQL, HTML, CSS, JS, Angular, Typescript, Data Structures and Algorithms"
        };
        var candidateUser6 = new CandidateUser
        {
            CompanyPosition = "DevOps engineer",
            ExpectedSalary = 400,
            HourlyRate = 10,
            ExperienceWorkDescription = 
                "As a DevOps Engineer and former experienced Full-Stack Engineer, I bring a unique combination of skills to the table. I have a proven track record of creating stable, secure, and highly performant web applications. Additionally, I am well-versed in implementing DevOps strategies, building infrastructure as code, creating efficient delivery pipelines, and managing deployment automation and release management effectively. My expertise in both development and operations makes me a valuable asset for any organization. I also have experience in administering Linux systems, which enables me to efficiently and effectively manage and maintain the underlying infrastructure of the applications I work on. I am also highly skilled in identifying and resolving a wide range of bugs and issues, both in application code and in the underlying infrastructure, ensuring minimal disruption to the overall performance and stability of the application",
            Country = "Ukraine",
            City = "Kyiv",
            LevelEnglish = englishLevelsDefault[4],
            CategoryWork = workCategoriesDefault[22],
            ApplicationUser = defaultUsers[8],
            CommunicationLanguage = communicationLanguagesData[1],
            IsProfileComplete = true,
            KeyWords =
                "C#, .NET, ASP.NET core, ASP.NET core MVC, ASP.NET core WEB API, EntityFrameworkCore, ADO.NET, Patterns, LINQ, MsSQL, T-SQL, HTML, CSS, JS, Angular, Typescript, Data Structures and Algorithms"
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
        var employerUser2 = new EmployerUser
        {
            CompanyName = "Genesis",
            CompanyPosition = "HR",
            CompanySiteLink = "https://gen.tech/",
            DoyCompanyLink = "https://jobs.dou.ua/companies/genesis-technology-partners/",
            AboutCompany =
                "Genesis is a co-founding company that builds global tech businesses together with outstanding entrepreneurs from CEE. We're one of the largest global app developers. Apps of Genesis companies have been downloaded by more than 300 mln users globally and are used by tens of millions of users monthly. Genesis has co-founded now independent tech companies such as Jiji, BetterMe, Headway. We've launched and are currently developing successful projects such as Obrio, Universe, Amomedia, Boosters, and others in our ecosystem. We have one of the best tech teams in Europe. As a result, we're constantly recognized as top IT employees in CEE and Ukraine.",
            ApplicationUser = defaultUsers[3]
        };

        if (!await _context.Users.AnyAsync())
        {
            foreach (var defaultUser in defaultUsers)
            {
                const string userPassword = "testPassword123";
                await _userManager.CreateAsync(defaultUser, userPassword);
            }

            await _userManager.AddToRoleAsync(defaultUsers[0], defaultRoles[0].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[1], defaultRoles[1].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[2], defaultRoles[2].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[3], defaultRoles[2].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[4], defaultRoles[1].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[5], defaultRoles[1].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[6], defaultRoles[1].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[7], defaultRoles[1].Name!);
            await _userManager.AddToRoleAsync(defaultUsers[8], defaultRoles[1].Name!);

            await _context.CandidateUsers.AddAsync(candidateUser);
            await _context.CandidateUsers.AddAsync(candidateUser2);
            await _context.CandidateUsers.AddAsync(candidateUser3);
            await _context.CandidateUsers.AddAsync(candidateUser4);
            await _context.CandidateUsers.AddAsync(candidateUser5);
            await _context.CandidateUsers.AddAsync(candidateUser6);
            await _context.EmployerUsers.AddAsync(employerUser);
            await _context.EmployerUsers.AddAsync(employerUser2);
            await _context.SaveChangesAsync();
        }

        // Seed default vacancies
        var vacanciesDefault = new List<Vacancy>
        {
            new()
            {
                VacancyTitle = "Trainee C# Developer",
                ShortDescription =
                    "DCT team looking for Trainee C# Developer! DCT team looking for Trainee C# Developer! DCT team looking for Trainee C# Developer!",
                LongDescription =
                    "<p>Requirements:- good knowledge of C#;</p><p></p><ul><li>good knowledge of C#;</li><li>good understanding of OOP;</li><li>understanding of SQL basics;</li><li>desire and ability to learn, target oriented</li></ul><br><p></p><p>Would be a plus:<br></p><p></p><ul><li>knowledge of design patterns, SOLID;<br></li><li>knowledge of WPF, UWP;<br></li><li>being a quick learner and have desire to improve technical skills;<br></li><li>1+ years of software development experience;<br></li></ul><p></p>",
                VacancyDomain = vacancyDomainDefault[3],
                WorkCategory = workCategoriesDefault[0],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[1],
                EnglishLevel = englishLevelsDefault[3],
                TypeOfCompany = typeOfCompanyDefault[1],
                EmployerUser = employerUser,
                FromSalary = 1200,
                ToSalary = 2000,
                ExperienceWork = 3,
                KeyWords = "C#, ASP.NET core, ASP.NET core MVC, ASP.NET core WEB API"
            },
            new()
            {
                VacancyTitle = "Marketing Specialist",
                ShortDescription =
                    "You're a fearless marketing enthusiast who loves generating bold ideas, making magic with conversion growth, and managing a marketing strategy? Well, Redwerk has the job just for you!",
                LongDescription =
                    "<p>Вітаємо!</p><p><br></p><p>Ми активно шукаємо Junior Back-End .NET розробника, який долучиться до нашої команди.</p><p><br></p><p>Про проект: Ігровий проект GTA5 на Rage MP (Це платформа, яка зв'язує сервер з клієнтом)</p><p><br></p><p>На одному сервері одночасно грають 1000-1500 людей, по-суті це є классичним лайф-сімом, тому кількість важких систем дуже висока.</p><p><br></p><p>В команді вже є два Back-End розробника, які з радістю будуть ділитися своїм досвідом, та допомогати швидве влитися до команди.</p><p><br></p><p>Пріорітет - Full-Time, можливо Part-time</p><p><br></p><p>Необхідно буде виконати тестове завдання, після успішного проходження загального і технічного інтерв'ю.</p><p>Наявність кейсів та досвіду можуть виключити цей пункт.</p><p><br></p><p><br></p><p>Обов'язки:</p><p>- Підтримка та покращения існуючого коду.</p><p>- Розробка геймплейних фічей на С#</p><p>- Код рев'ю та рефакторінг</p><p>- Спілкування та взаємодія з різними командами (Гейм-Дизайнери, Дизайнери, 3Д спеціалісти)</p><p><br></p><p>Вимоги:</p><p>- Бажання розвиватись профессійно, та бажання делівірети якісний результат для гравців. Розуміння спеціфіки Gaming розробки</p><p>- Client-Server application understanding</p><p>- OOP</p><p>- C#</p><p>- Git</p><p>- SQL</p><p><br></p><p>Буде перевагою:</p><p>- HTML</p><p>- CSS</p><p>- JS</p><p>- Windows Server</p><p>- MySQL</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[5],
                WorkCategory = workCategoriesDefault[19],
                HowToWork = howToWorkDefault[3],
                Relocate = relocateDefault[0],
                CandidateRegion = candidateRegionsDefault[3],
                EnglishLevel = englishLevelsDefault[1],
                TypeOfCompany = typeOfCompanyDefault[2],
                EmployerUser = employerUser,
                FromSalary = 200,
                ToSalary = 500,
                ExperienceWork = 2,
                KeyWords = "Marketing, Specialist"
            },
            new()
            {
                VacancyTitle = "Middle Python Developer",
                ShortDescription =
                    "Do you want to make the world a better place?  Join us and let’s develop great healthcare projects together!  ",
                LongDescription =
                    "<p>About the project: Integrated Primary Care Delivery Platform that automates patient communication, maximizing access by dynamically matching patients with the care they need.</p><p>Specialization: Healthcare</p><p>Project technology stack: Python</p><p><br></p><p>About you:</p><p>— 3+ years of commercial programming experience</p><p>— Knowledge of MySQL and Docker</p><p>— English - Upper-Intermediate / В2</p><p>— Independence and determination</p><p>As a plus:</p><p>— Prior experience in healthcare domain and HIPAA compliance</p><p><br></p><p>Your responsibilities will include:</p><p>— write easy-to-maintain codes, following coding best practices</p><p>— code review</p><p>— communication with the team and the client</p><p>— daily stand-ups / Retros / Plannings etc</p><p><br></p><p>We offer:</p><p>— Working at a cutting-edge project</p><p>— Mature and efficient processes</p><p>— Friendly and supportive work environment</p><p>— Competitive salary and benefits package</p><p>— Room for personal and professional growth</p><p>— Zero bureaucracy</p><p>— 18 business days of paid vacation + public holidays compensation</p><p>— Insurance Fund of the company</p><p>— Coverage of all professional studies</p><p>— Сoverage of sick leaves, sports activities, and English language courses</p><p>— Regular team buildings and corporate events</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[10],
                WorkCategory = workCategoriesDefault[3],
                HowToWork = howToWorkDefault[3],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[4],
                TypeOfCompany = typeOfCompanyDefault[3],
                EmployerUser = employerUser,
                FromSalary = 500,
                ToSalary = 1200,
                ExperienceWork = 2,
                KeyWords = "Python, Middle, Middle python developer, Python developer"
            },
            new()
            {
                VacancyTitle = "Java Team Lead",
                ShortDescription =
                    "KYIV. OFFICE JOB    Delasport is looking for a Java Team Lead to join our new project from scratch    PLANNED TECHNOLOGY STACK  Microservice architecture, Java 17, Spring 3, Kafka, Google Cloud, Kubernetes  ",
                LongDescription =
                    "<p>RESPONSIBILITIES</p><p><br></p><p>- Taking an active part in product development from scratch (hands-on functionality development ~50% of the time)</p><p>- Taking an active part in improvements and optimizations to the systems, being in charge of the code quality</p><p>- Leading a development team</p><p>- Ensuring the highest standards of the team’s performance are reachable</p><p>- Planning, prioritizing, estimating, and executing tasks</p><p>- Facilitating team’s technical sessions</p><p>- Collaborating with business leads across the company to define milestones and deliveries for new functionality to be added to the product</p><p>- Being in charge of people management needs (hiring, onboarding, mentoring, career development, performance evaluation, one-on-one meetings)</p><p><br></p><p>REQUIREMENTS</p><p><br></p><p>- 7+ years of commercial development experience using Java-related technologies</p><p>- Experience in managing development teams</p><p>- Experience in process development/optimization</p><p>- Ability to work independently with defined problems on a middle-senior level in certain areas of technical or business expertise</p><p>- Proactive and result-oriented mindset</p><p>- Strong problem-solving skills</p><p>- At least Upper-Intermediate English level</p><p><br></p><p>NICE TO HAVE😎</p><p><br></p><p>- Experience in product companies</p><p>- Knowledge of JavaScript/React</p><p><br></p><p>WHAT WE CAN OFFER YOU</p><p>- Modern office in Podil with an uninterruptible power supply and the Internet</p><p>- Personal time off (21 business days of paid vacation, unlimited sick -leaves, paid days on special occasions)</p><p>- Public holidays</p><p>- Health insurance with the broker which is available from the first month of cooperation</p><p>- Modern technical equipment</p><p>- Ukraine-based educational programs</p><p>- Sports activities reimbursement</p><p>- Corporate entertainments</p><p>- Happy hours on Fridays</p><p>- PE accounting and support</p><p><br></p><p>Does this sound like you? Come join us!</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[3],
                WorkCategory = workCategoriesDefault[2],
                HowToWork = howToWorkDefault[1],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[3],
                TypeOfCompany = typeOfCompanyDefault[0],
                EmployerUser = employerUser,
                FromSalary = 2000,
                ToSalary = 5000,
                ExperienceWork = 8,
                KeyWords = "Java, Team Lead, Senior developer"
            },
            new()
            {
                VacancyTitle = "Unity Developer",
                ShortDescription =
                    "We are creating projects that erase the line between education and gaming.  Collaboration is highly encouraged here, so you’ll have a direct impact on game design and shaping the overall project direction. ",
                LongDescription =
                    "<p>What you’ll do</p><p>- As part of a small team, you’ll work on a wide assortment of tasks. Plug in where needed, working on core systems one day and implementing VFX the next;</p><p>- Write maintainable code with an eye on performance;</p><p>- Focus on reusable and designer-friendly architecture;</p><p>- Demonstrate proficiency in debugging and performance profiling;</p><p>- Create Unity editor scripts/tools to improve production workflows;</p><p>- Self QA your experiences and diagnose bugs that pop up;</p><p>- Contribute to game design and planning conversations;</p><p>- Work with the rest of the team to add/update new features, content and gameplay.</p><p><br></p><p>What you’ll need</p><p>- At least 3 years of experience in Unity game development;</p><p>- Experience with 2D/3D graphics;</p><p>- Strong knowledge of C#;</p><p>- Clean code writing skills, code refactoring;</p><p>- Experience in development for mobile platform (iOS/Android);</p><p>- Good knowledge of OOP, SOLID, design patterns;</p><p>- Code profiling and optimisation experience;</p><p>- Familiar with client server architecture (REST, Socket);</p><p>- Have experience in full cycle game development from task assignment, code review to deployment and publishing;</p><p>- Experience with Agile workflow;</p><p>- Comfortable working with new APIs and concepts in this always-changing industry;</p><p>- Good communication skills and English is a big plus!</p><p><br></p><p>BONUS POINTS</p><p>- Experience working with Unity’s Scriptable Render Pipeline;</p><p>- Experience with Unity VFX Graph;</p><p>- Experience with Unity Shader Graph.</p><p><br></p><p>SHOW US WHAT YOU GOT!</p><p>- Links to any Unity projects you’ve worked on, and what you contributed.</p><p>- Videos, GIFs, JPEGs of your best, juiciest work.</p><p>- Links/media of projects you did just for fun / learning (Itch.io page, a downloadable build, etc).</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[4],
                WorkCategory = workCategoriesDefault[27],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[3],
                EnglishLevel = englishLevelsDefault[1],
                TypeOfCompany = typeOfCompanyDefault[2],
                EmployerUser = employerUser,
                FromSalary = 600,
                ToSalary = 1000,
                ExperienceWork = 1,
                KeyWords = "Unity, C#, Unity developer"
            },
            new()
            {
                VacancyTitle = "Front-end Developer",
                ShortDescription =
                    "We are looking for a mid-level front-end application developer (beneficial to have some backend exposure or experience) who is motivated to work as part of a team to create, maintain, and add new features for a suite of web-based applications.",
                LongDescription =
                    "<p>Responsibilities:</p><p>• Build awesome frontend solutions. Make application wonderful and fast.</p><p>• Build reusable code and libraries for future use</p><p>• Optimize application for maximum speed and scalability</p><p>• Assure that all user input is validated before it is submitted to the back-end</p><p>• Collaborate with other team members and stakeholders</p><p><br></p><p>Skills And Qualifications:</p><p>• 5+ years experience in Front-end web development</p><p>• JS</p><p>• Proficient understanding of cross-browser compatibility issues and ways to work around them</p><p>• Proficient understanding of code versioning tool - Git</p><p>• Experience creating and using mock databases and APIs during development</p><p>• Deep expertise in HTML, CSS</p><p>• Self –starter and able to identify both problems and solutions to problems.</p><p><br></p><p>What you’ll get!</p><p>●Fun, collaborative, friendly team</p><p>●Exciting and stable startup</p><p>●Working on the latest technologies</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[2],
                WorkCategory = workCategoriesDefault[1],
                HowToWork = howToWorkDefault[1],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[0],
                EnglishLevel = englishLevelsDefault[0],
                TypeOfCompany = typeOfCompanyDefault[0],
                EmployerUser = employerUser,
                FromSalary = 2000,
                ToSalary = 3000,
                ExperienceWork = 3,
                KeyWords = "Front-end developer, JS, JavaScript, Angular, React"
            },
            new()
            {
                VacancyTitle = "Junior Graphic Designers",
                ShortDescription =
                    "Ви будете працювати в Holy Water — паблішер заснованих на даних застосунків та рольових мобільних ігор. Запустившись у 2020 році, компанія виросла до понад 100 осіб та створила декілька продуктів, якими користуються понад 10 мільйонів користувачів",
                LongDescription =
                    "<p>Продукти компанії</p><p><br></p><p>My Fantasy — гра у жанрі інтерактивних історій, яка за один день отримала 120 тисяч завантажень. Виручка на місяць — один мільйон доларів. Таких результатів команда досягла запустивши продукт за дев’ять місяців без експертизи в геймдеві з командою у вісім людей. З часом My Fantasy стала лідером серед ігор з інтерактивними історіями в Австралії та увійшла в п’ятірку найкращих у своїй ніші у світі.</p><p><br></p><p>Passion — застосунок, де зібрано романтичні історії на будь-який смак.</p><p><br></p><p>Ви будете працювати у команді, що займається просуванням My Fantasy. У команді Марти Хоми ми працюємо над продуктами впізнаваними по всьому світу! Разом ми зможемо відстежувати сучасні тренди, аналізувати ринок, створювати креативи - які чіпляють, а також ефективно працювати з різними рекламними платформами.</p><p><br></p><p>Зараз ми зосереджені на масштабуванні команди та пошуку людей, які допоможуть вивести наш застосунок MyFantasy на нові висоти. Якщо ви смілива, працьовита, допитлива, самосвідома людина, яка не боїться робити помилки та вчитися на них, давай поспілкуємось!</p><p><br></p><p>Зараз команда шукає Junior Graphic Designers, котрий буде тісно працювати з креативною командою та створювати високоякі матеріали для креативів та інших напрямів маркетингу.</p><p><br></p><p>Чим ви будете займатися:</p><p><br></p><p>Здійснювати порізку ассетів.</p><p>Здійснювати підготовку графічних матеріалів для відео команди.</p><p>Здійснювати підготовку графічних матеріалів для кастомних сторінок у сторах.</p><p>Брати участь у розробці дизайнів для Playable ADs.</p><p>Що потрібно, аби приєднатися:</p><p><br></p><p>Досвід в якості Graphic Designers від 6 місяців.</p><p>Володіння Photoshop та Figma на високому рівні.</p><p>Наявність художніх скілів (вміння малювати).</p><p>Креативність та самомотивованість.</p><p>Вміння приймати та давати зворотній зв’язок.</p><p>Що ми пропонуємо:</p><p><br></p><p>Гнучкий графік роботи, можливість працювати віддалено з будь-якої безпечної точки світу.</p><p>За потреби — допомога з релокацією в безпечні місця, консультація з питань легального перебування закордоном, інформація щодо підтримки громадян третіми країнами, допомога з пошуком житла чи бронюванням квитків.</p><p>Послуги корпоративного лікаря, а після випробного терміну — медичне страхування в Україні.</p><p>Є можливість звернутися за консультацією до психолога.</p><p>20 робочих днів оплачуваної відпустки на рік, необмежена кількість лікарняних.</p><p>Уся необхідна для роботи техніка.</p><p>Онлайн-бібліотека, регулярні лекції від спікерів топрівня, компенсація конференцій, тренінгів та семінарів.</p><p>11 професійних внутрішніх ком’юніті для вашого кар’єрного розвитку (DevOps, Front-end, Back-end, QA, Product, Analytics, Marketing, Design, PR та Recruiting)</p><p>Комфортні офіси на київському Подолі з надійними укриттями. В офісах можна не турбуватися про рутину: тут на вас чекають сніданки, обіди, безліч снеків та фруктів, лаунжзони, масаж та інші переваги офісного життя 🙂</p><p>Етапи відбору</p><p><br></p><p>1. Первинний скринінг. Рекрутер ставить декілька запитань (телефоном або в месенджері), аби скласти враження про ваш досвід і навички перед співбесідою.</p><p><br></p><p>2. Тестове завдання. Підтверджує вашу експертизу та показує, які підходи, інструменти й рішення ви застосовуєте в роботі. Ми не обмежуємо вас у часі та ніколи не використовуємо напрацювання кандидатів без відповідних домовленостей.</p><p><br></p><p>3. Співбесіда з менеджером. Всеохопна розмова про ваші професійні компетенції та роботу команди, в яку подаєтесь.</p><p><br></p><p>4. Бар-рейзинг. На останню співбесіду ми запрошуємо одного з топменеджерів екосистеми Genesis, який не працюватиме напряму з кандидатом. У фокусі бар-рейзера — ваші софт-скіли та цінності, аби зрозуміти, наскільки швидко ви зможете зростати разом з компанією.</p><p><br></p><p>Дбайливий онбординг</p><p><br></p><p>У перший день ви познайомитеся з командою та обговорите з керівником ваші цілі на випробний термін. Вас підключать до всіх каналів комунікації, додадуть у неформальні чати, нададуть документи та інструкції, щоби ви ознайомилися з процесами в команді. Якщо потрібно, на допомогу призначать ментора чи бадді, який допоможе адаптуватися на новому місці та відповість на всі запитання. Детальніше про процес онбордингу можна почитати тут.</p><p><br></p><p>Культура компанії</p><p><br></p><p>У своїй роботі команда спирається на шість ключових цінностей: постійне зростання, внутрішня мотивація, завзятість і гнучкість, усвідомленість, свобода та відповідальність, орієнтація на результат.</p><p>Маєте пропозиції щодо того, як вдосконалити продукт чи процеси? Супер, нумо пробувати!</p><p>Водночас кожен несе відповідальність за свої дії, робить висновки та готовий чути фідбек.</p><p>Наша стратегічна мета — стати єдинорогом до 2026 року.</p><p>Відгукнулося? Надсилайте резюме — з нетерпінням чекаємо на знайомство!</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[0],
                WorkCategory = workCategoriesDefault[17],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[2],
                TypeOfCompany = typeOfCompanyDefault[2],
                EmployerUser = employerUser,
                FromSalary = 400,
                ToSalary = 1200,
                ExperienceWork = 1,
                KeyWords = "Designer, Graphic designer, Junior"
            },
            new()
            {
                VacancyTitle = "Senior Web Engineer (Full Stack) Ruby",
                ShortDescription =
                    "Danavero Inc. is looking for Senior Web Engineer (Full Stack) to join our development team. We are looking for a candidate who is capable to learn fast and work directly with our customers to improve and support product, who is passionate about",
                LongDescription =
                    "<p>Danavero is seeking a Senior Web Engineer who enjoys challenging problems to join our team. Be a part of our growing startup, creating secure and highly scalable web applications. As a senior member of growing engineering team, you will:</p><p><br></p><p>Participate in the design/planning process for our core applications</p><p>Write performant, scalable, fault tolerant code in a high traffic environment</p><p>Be a leader in our Agile, team oriented environment, providing code review, technical advice, and mentoring</p><p>Value automation and strive to find the better way</p><p>Work in a fast-paced but flexible startup atmosphere where you’re assessed on results and given the freedom to achieve them</p><p>Work hard and smart with us to build a world class SaaS platform that will forever change eCommerce, and have fun doing it</p><p><br></p><p>Requirements</p><p>Deep understanding of ruby, rails, javascript, html, css, and related technologies</p><p>Extensive RDBMS/SQL knowledge</p><p>Strong experience implementing automated tests</p><p>Proficient in applying cs concepts, algorithms, patterns, anti-patterns, and best practices</p><p>Professional experience working in a SaaS environment</p><p>Being a great teammate</p><p><br></p><p>Bonus Points</p><p>Familiarity with ci/cd and devops concepts, especially in an AWS environment</p><p>Containerization, specifically Docker/Kubernetes</p><p>Marketing tech/eCommerce experience</p><p>Google Analytics, SEO, eCommerce platform (ex. Magento) experience</p><p>Understanding of digital coupons, SEM, affiliate marketing, and other online marketing concepts</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[15],
                WorkCategory = workCategoriesDefault[9],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[4],
                TypeOfCompany = typeOfCompanyDefault[3],
                EmployerUser = employerUser2,
                FromSalary = 3000,
                ToSalary = 6000,
                ExperienceWork = 8,
                KeyWords = "Ruby, WebDev, Senior, Full stack"
            },
            new()
            {
                VacancyTitle = "C++ Graphics Developer",
                ShortDescription =
                    "Big product software company is looking for a C++ Graphics Developer. Remote work, high salary + financial bonuses (up to 100% of the salary), regular salary review, new interesting projects, good working conditions.",
                LongDescription =
                    "<p>REQUIREMENTS:</p><p>- 2+ years in C++ programming;</p><p>- Theoretical knowledge of Vulkan / OpenGL / Unreal Engine;</p><p>- Higher education;</p><p>- Technical English (higher level is advantage).</p><p><br></p><p>COMPANY OFFERS:</p><p>- Employment under gig-contract, all taxes are paid;</p><p>- Flexible working hours;</p><p>- 28 days of paid vacation + 15 days at your own expense;</p><p>- Paid sick leave;</p><p>- Medical insurance (with dentistry and optics), including the family;</p><p>- Free English courses;</p><p>- Career and professional growth;</p><p>- External trainers (the best representatives of the IT sector in the country);</p><p>- Own base of courses and trainings;</p><p>- Permanent discount on gym / pool membership;</p><p>- Office in the Kyiv city centre / remotely;</p><p>- Provision of necessary up-to-date equipment;</p><p>- Regular salary review and financial bonuses (up to 100% of the salary);</p><p>- Bonuses for wedding, birth of children and other significant events;</p><p>- Paid maternity leave;</p><p>- Tea, coffee, water, snacks;</p><p>- Bicycle parking.</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[10],
                WorkCategory = workCategoriesDefault[8],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[0],
                TypeOfCompany = typeOfCompanyDefault[1],
                EmployerUser = employerUser2,
                FromSalary = 300,
                ToSalary = 1000,
                ExperienceWork = 0,
                KeyWords = "C++, Graphic, Graphic developer"
            },
            new()
            {
                VacancyTitle = "ASP.NET Developer",
                ShortDescription =
                    "We're looking for an experienced ASP.NET Core Developer to work with our partners and customers on building innovative products. You'll be responsible for developing and maintaining software applications, integrating them with existing systems",
                LongDescription =
                    "<p>Requirements</p><p><br></p><p>At least 4 years of experience with ASP.NET</p><p>Knowledge of ASP.NET Core</p><p>Strong knowledge of OOP</p><p>Familiarity with Azure</p><p>Upper-Intermediate English level</p><p><br></p><p>Responsibilities:</p><p><br></p><p>Develop the ongoing platform</p><p>Writing tech specifications &amp; architecture</p><p>Closely cooperate with other team members owners/developers/designers)</p><p>Primarily work on API development</p><p><br></p><p>We offer</p><p><br></p><p>Full-time remote work</p><p>24 days vacation per year</p><p>Knowledge sharing,</p><p>Covering conference expenses</p><p>Flexible worktime</p><p>People-oriented management without bureaucracy</p><p><br></p><p><br></p><p>If you're looking for a challenging and rewarding role that allows you to work with cutting-edge technology, we'd love to hear from you. Please submit your resume.</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[0],
                WorkCategory = workCategoriesDefault[0],
                HowToWork = howToWorkDefault[1],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[4],
                TypeOfCompany = typeOfCompanyDefault[1],
                EmployerUser = employerUser2,
                FromSalary = 100,
                ToSalary = 400,
                ExperienceWork = 0,
                KeyWords = "Trainee, C#, .NET, Asp.Net core"
            },
            new()
            {
                VacancyTitle = "Full-Stack .Net Engineer",
                ShortDescription =
                    "Since 1986, our client has been working with hotel developers, owners, and lenders to make their projects successful.",
                LongDescription =
                    "<p>Our client developed a complete intelligence platform where customers can order room service using Alexa. The project has a lot of integrations with external APis like Booking.com, etc. for pulling at the moment but in future it should be two-way collaboration.</p><p><br></p><p>TechStack:</p><p>- Backend development - .Net Core</p><p>- Frontend development - Angular 12</p><p>- Clouds - AWS</p><p><br></p><p>Requirements</p><p>- At least 2 years of experience developing application using C#, ASP.Net Core for backend and Angular 12 for frontend</p><p>- Experience with AWS, especially lambdas</p><p>- Understanding PostgresQL database</p><p>- Excellent analytical and problem-solving skills</p><p>- A collaborative and dedicated team player</p><p>- At least Intermediate English level (both verbal and written)</p><p><br></p><p>Responsibilities:</p><p>- Drive backend and frontend development</p><p>- Collaborate with other programming engineers across the organization to develop best practices</p><p>- Write unit tests</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[5],
                WorkCategory = workCategoriesDefault[0],
                HowToWork = howToWorkDefault[3],
                Relocate = relocateDefault[0],
                CandidateRegion = candidateRegionsDefault[3],
                EnglishLevel = englishLevelsDefault[1],
                TypeOfCompany = typeOfCompanyDefault[2],
                EmployerUser = employerUser2,
                FromSalary = 1500,
                ToSalary = 2200,
                ExperienceWork = 3,
                KeyWords = "C#, Full-stack, .NET engineer"
            },
            new()
            {
                VacancyTitle = "Full stack Developer (NodeJS+Angular)",
                ShortDescription =
                    "We are looking for an experienced Back-end Developer (NodeJS), who will join an international team that is working on an exciting and valuable financial project. We believe in the power of blockchain technology to revolutionize the world across",
                LongDescription =
                    "<p>Qualifications:</p><p>-Experience with Node.js, Typescript (2+ years);</p><p>-Experience with the related library - Sequelize;</p><p>-Knowledge of relational databases (PostgreSQL, MSSQL, etc.);</p><p>-Experience with REST;</p><p>-Experience with Cloud services: AWS (Lambda, S3, Cognito, SQS, CloudWatch), Serverless;</p><p>-Expertise in testing frameworks: Jest, Cucumber;</p><p>-Experience with integrating 3rd party APIs;</p><p>-Ability to write and communicate effectively;</p><p>-Upper-Intermediate English level (or higher) is a must-have.</p><p><br></p><p>Responsibilities:</p><p>-Work independently on the back-end development for the digital solutions;</p><p>-Develop, test, and support new features;</p><p>-Brainstorm new features with a team and client;</p><p>-Managing teams and helping them prioritize tasks;</p><p>-Design and develop APIs;</p><p>-Be able to configure AWS Cognito pools, SQS queues, Lambda functions.</p><p><br></p><p>Why IntellectEU:</p><p>-Сompetitive salary and benefits package.</p><p>-Work alongside a passionate and friendly team in an innovative, casual, positive, and open work environment.</p><p>-IntellectEU believes in both personal and professional growth and will work alongside you to ensure that you reach your goals in both areas.</p><p>-Learning and development plan - free meetups, English classes.</p><p>-Possibility to choose your workspace either remote or a combination of your home and our office.</p><p>-Flexible working hours and adjustable work/life balance. Support for a healthy lifestyle, running events, and challenges.</p><p>-Corporate events, Birthday presents, yearly gifts.</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[10],
                WorkCategory = workCategoriesDefault[5],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[4],
                TypeOfCompany = typeOfCompanyDefault[1],
                EmployerUser = employerUser2,
                FromSalary = 1000,
                ToSalary = 3000,
                ExperienceWork = 1,
                KeyWords = "Full stack, Node.js, JS, CSS, HTML"
            },
            new()
            {
                VacancyTitle = "JavaScript Full-Stack Tech Lead",
                ShortDescription =
                    "We are looking for a JavaScript Full-stack Team Lead to add to our team for development support and assistance. The vacancy is open due to the amount of the work coming in and the need of strengthening of the development team.  ",
                LongDescription =
                    "<p>Responsibilities:</p><p>-Lead a department of developers to design, develop and maintain web applications using React and Node.js;</p><p>-Write clean, maintainable, and efficient code for both front-end and back-end;</p><p>-Write and maintain documentation for the application and its APIs;</p><p>-Collaborate with other developers, designers, and stakeholders to deliver quality solutions;</p><p>-Participate in code reviews to ensure code quality and adherence to development standards;</p><p>-Troubleshoot and debug applications;</p><p>-Optimize application for maximum speed and scalability;</p><p>-Stay up-to-date with new technologies and industry trends;</p><p>-Participate in daily stand-up meetings and other Agile ceremonies;</p><p>-Assist in the estimation of development tasks and timelines;</p><p>-Mentor junior developers and provide guidance on best practices and technical decisions;</p><p>-Manage project timelines and deliverables, ensuring that projects are completed on time and within budget.</p><p><br></p><p>Qualification &amp; Skills:</p><p>-5+ years of experience with full stack web development, including experience with React and Node.js;</p><p>-Advanced knowledge of the JavaScript ecosystem. In particular, current frameworks, libraries, and the potential architecture of solutions and good practices;</p><p>-Advanced knowledge and understanding of RESTful APIs;</p><p>-Advanced understanding the main differences and experience with Webpack, gulp;</p><p>-Experience with PostgreSQL and MongoDB databases;</p><p>-Experience with GraphQL;</p><p>-Experience with cloud services based on Amazon Web Services (ideally EC2, ELB, S3, Lambda), serverless computing;</p><p>-Experience with testing frameworks like Jest, Enzyme, and Mocha;</p><p>-Experience with Node.js application scaling;</p><p>-Experience of leading a project while staying ‘hands-on’ at the same time;</p><p>-Practical knowledge of CI/CD tools, Git, Gitflow, server-side cloud solutions;</p><p>-Experience of working throughout the entire production process of applications (from requirements analysis, through programming and testing, and up to final delivery);</p><p>-Good written and verbal communication skills in English (Upper Intermediate +) are a MUST.</p><p><br></p><p>Will be a plus:</p><p>-Experience as a Tech/Team leader;</p><p>-Experience with Vue.js and Angular will be a huge advantage.</p><p><br></p><p>What we offer:</p><p>- Remote work, partially flexible hours;</p><p>- Full time employment, 8 hours / 5 days a week;</p><p>- Paid 10 days of vacation + 5 days of sick leave at the expense of the company in the first year</p><p>of work (active 3 months after the start of the full time).</p><p>- Until then, you can take days off at your own expense.</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[2],
                WorkCategory = workCategoriesDefault[1],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[2],
                TypeOfCompany = typeOfCompanyDefault[3],
                EmployerUser = employerUser2,
                FromSalary = 4500,
                ToSalary = 6000,
                ExperienceWork = 5,
                KeyWords = "JS, Full-stack, Tech lead"
            },
            new()
            {
                VacancyTitle = "React Developer with Headless Magento Experience",
                ShortDescription =
                    "We are looking for a skilled React Developer with experience in Headless Magento development. As a React Developer, you will be responsible for creating and implementing user interface components using React.js workflows and integrating Magento",
                LongDescription =
                    "<p>Responsibilities:</p><p>- Developing new user-facing features using React.js</p><p>- Building reusable components and libraries for future use</p><p>- Integrating with Magento APIs to create a headless e-commerce solution</p><p>- Optimizing components for maximum performance across a vast array of web-capable devices and browsers</p><p>- Collaborating with other team members and stakeholders to create a seamless user experience</p><p>- Participating in code reviews and maintaining code quality standards</p><p><br></p><p>Requirements:</p><p>- Proven work experience as a React Developer</p><p>- Experience with Magento 2 and its APIs</p><p>- Strong proficiency in JavaScript, including DOM manipulation and the JavaScript object model</p><p>- Familiarity with RESTful APIs</p><p>- Experience with React workflows such as Redux and Flux</p><p>- Knowledge of modern authorization mechanisms, such as JSON Web Token</p><p>- B2 level English communication skills, both written and verbal</p><p><br></p><p>If you meet these requirements and have a passion for creating innovative web applications, we encourage you to apply for this exciting opportunity.</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[2],
                WorkCategory = workCategoriesDefault[1],
                HowToWork = howToWorkDefault[0],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[2],
                TypeOfCompany = typeOfCompanyDefault[0],
                EmployerUser = employerUser2,
                FromSalary = 1200,
                ToSalary = 4000,
                ExperienceWork = 4,
                KeyWords = "React, Magento, Middle, Profile"
            },
            new()
            {
                VacancyTitle = "DevOps Engineer",
                ShortDescription =
                    "Стабільна фінтех-компанія з перспективними планами розвитку зараз посилює команду і запрошує до співпраці — DevOps Engineer  ",
                LongDescription =
                    "<p>Команда (до 20 осіб):</p><p><br></p><p>Backend: 3 розробники;</p><p>Frontend: 2;</p><p>Fullstack: 4 (від junior до senior).</p><p>3 devops, 2 BA, sys.admin, tech supp та PM.</p><p><br></p><p><br></p><p>Вимоги:</p><p><br></p><p>• Базові знання bash, чітке розуміння CI\\CD;</p><p>• Kubernetes (Weave CNI, MetalLB, GlusterFS, heapster);</p><p>• ELK;</p><p>• Linux: CentOS;</p><p>• Кластер RabbitMQ;</p><p>• Grafana, Prometheus;</p><p>• Percona Monitoring Management, Percona XtraDB Cluster;</p><p>• Gitlab/CI;</p><p>• AWS: EC2, RDS, VPC, ELB;</p><p>• Proxmox, VMware;</p><p>• Досвід налаштування nginx\\PHP;</p><p>• Знання англійської мови: письмова - Upper-intermediate+, розмовна - Intermediate.</p><p><br></p><p><br></p><p><br></p><p>Обов'язки:</p><p><br></p><p>• Підтримка та розвиток відмовостійкого кластера віртуалізації (ECS, Proxmox);</p><p>• Моніторинг доступності (Prometheus);</p><p>• Налаштування логів;</p><p>• Управління спеціальними інструментами та вдосконалення заходів безпеки (Firewall, Anti-DDoS, IDS / IPS, WAF);</p><p>• Щорічний аудит PCI DSS;</p><p>• Написання та оновлення ролей для автоматизованого розгортання (Ansible, Jenkins, Git, shell script).</p><p><br></p><p><br></p><p>Буде плюсом:</p><p><br></p><p>• AWS: ECS, ECR;</p><p>• Jenkins;</p><p>• Ansible, Terraform;</p><p>• Linux: Debian;</p><p>• Juniper;</p><p>• Попередній досвід у налаштуванні проксі для балансування веб-серверів та бази даних (haproxy, nginxl).</p><p><br></p><p><br></p><p>Ми пропонуємо:</p><p><br></p><p>— Повний Remote з гнучким графіком роботи;</p><p>— Цікавий і масштабний проєкт у сфері онлайн платежів;</p><p>— Гідний рівень фінансової мотивації;</p><p>— Професійний розвиток, тренінги та профільні навчання.</p><p><br></p><p><br></p><p><br></p><p>Етапи: ⭕️ Ознайомчий дзвінок ⭕️ тех. кол з командою, де беруть участь DevOps, PM, CTO</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[10],
                WorkCategory = workCategoriesDefault[22],
                HowToWork = howToWorkDefault[1],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[3],
                EnglishLevel = englishLevelsDefault[3],
                TypeOfCompany = typeOfCompanyDefault[2],
                EmployerUser = employerUser2,
                FromSalary = 1000,
                ToSalary = 2000,
                ExperienceWork = 1,
                KeyWords = "DevOps Engineer, DevOps"
            },
            new()
            {
                VacancyTitle = ".NET Infrastructure Developer",
                ShortDescription =
                    "The .NET infrastructure developer is responsible for the development and maintenance of several legacy C#/.NET applications used by the company. The majority of these are back-end infrastructure projects, though occasional limited GUI development",
                LongDescription =
                    "<p>The .NET infrastructure developer is responsible for the development and maintenance of several legacy C#/.NET applications used by the company. The majority of these are back-end infrastructure projects, though occasional limited GUI development is also required. The role also involves contributing to software architecture decisions. This role reports directly to the executive level or their designates and will involve close collaboration with members of the Data, Execution, and Research teams. Success in this role will involve ongoing improvements to our data infrastructure software, understanding of data flow, and optimization of existing processes.</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[7],
                WorkCategory = workCategoriesDefault[0],
                HowToWork = howToWorkDefault[1],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[0],
                EnglishLevel = englishLevelsDefault[4],
                TypeOfCompany = typeOfCompanyDefault[1],
                EmployerUser = employerUser,
                FromSalary = 500,
                ToSalary = 2000,
                ExperienceWork = 1,
                KeyWords = ".NET, Infrastructure, Developer"
            },
            new()
            {
                VacancyTitle = "Full-Stack (React+Python) Engineer",
                ShortDescription =
                    "We are looking for Full-stack Engineer (React+Python) for a world-famous product for the full remote from Poland.",
                LongDescription =
                    "<p>What will you do?</p><p>🔹React to Python (60/40). Feature development for the internal platform</p><p>🔹Implement UI flows, views, and components using React, HTML, CSS, and other front-end languages and libraries</p><p>🔹Work with services/APIs and back-end teams to ensure that UI dependencies are communicated, understood, tested, and delivered</p><p>🔹Microservices implementation</p><p>🔹Troubleshooting and fixing the code bugs</p><p>🔹Coordination and communication within the team</p><p><br></p><p>What should you have?</p><p>🔸3+ years of experience in the IT area</p><p>🔸Hands-on business experience with React, JS</p><p>🔸Hands-on business experience with Python 2+</p><p>🔸Knowledge of AWS and K8s</p><p>🔸Experience with CD Platforms is a plus</p><p>🔸Good speaking English level</p><p><br></p><p>⏰Working conditions:</p><p>Be available from 7 am -12 pm PST!</p><p><br></p><p>What do we offer?</p><p>☑️Paid vacation, sick leave (without sickness list)</p><p>☑️Official state holidays — 11 days considered public holidays</p><p>☑️Professional growth while attending challenging projects and the possibility to switch your role, master new technologies and skills with company support</p><p>☑️Flexible working schedule: 8 hours per day, 40 hours per week. 2-3h PST overlap</p><p>☑️Personal Career Development Plan (CDP)</p><p>☑️Employee support program (Discount, Care, Heals, Legal compensation)</p><p>☑️Paid external training, conferences, and professional certification that meets the company’s business goals</p><p>☑️Internal workshops &amp; seminars</p><p>☑️Corporate library (Paper/E-books) and internal English classes</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[6],
                WorkCategory = workCategoriesDefault[3],
                HowToWork = howToWorkDefault[0],
                Relocate = relocateDefault[1],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[2],
                TypeOfCompany = typeOfCompanyDefault[0],
                EmployerUser = employerUser2,
                FromSalary = 2000,
                ToSalary = 4500,
                ExperienceWork = 3,
                KeyWords = "Full stack, React, Python"
            },
            new()
            {
                VacancyTitle = "Python Software Engineer",
                ShortDescription =
                    "We are looking for a Python Software Engineer who will become a part of our innovative and growing company and help us push the boundaries of what is possible in ML technology.",
                LongDescription =
                    "<p>⚙️ Product Stack:</p><p>Python 3, Redis, BentoML, MLflow, k8s &amp; helm</p><p><br></p><p>Workflow:</p><p>• team consists of 3 Engineers, QA and Head of ML Production; the seniority level of the team is Middle/Senior, but we prefer not to use titles at all</p><p>• we use working processes. There are no scrum masters. The team decides how to work and what technologies to choose</p><p>• no micromanagement. We have no time or desire for this</p><p>• unlimited vacation time. We trust each other</p><p>• now we work remotely and smart, instead of 9 to 5</p><p><br></p><p>What will you do:</p><p>• deliver new ML models to production</p><p>• optimize performance of Python ML related code</p><p>• ensure stable operation of the existing system</p><p><br></p><p>You:</p><p>• have 3+ years of experience in software engineering</p><p>• have 1+ years of experience with Python</p><p>• have experience with Google Cloud or AWS infrastructure</p><p>• have a good understanding of async code, multithreading/multiprocessing</p><p>• are familiarity with SQL and NoSQL databases</p><p><br></p><p>Nice to have basic knowledge and interest in ML related concepts, experience in deploying ML models to production, working experience in high load/high performance distributed projects and in projects involving image/video processing. It’s not among must-haves, still, such knowledge will сome in handy further</p><p><br></p><p>❗We’re not asking you to be a 100% empath or to love everybody. Just don’t be evil, rude, or selfish. And try to communicate with the team like a human being</p><p>It’s important to us that the team feels comfortable around you!</p><p><br></p><p>Hiring process:</p><p>✅ Intro call with a Recruiter - ✅ Technical Screen - ✅ Technical Interview - ✅ Offer</p><p><br></p><p>If you are passionate about ML and have the skills and experience we are looking for, apply now and join our team at Reface!</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[9],
                WorkCategory = workCategoriesDefault[3],
                HowToWork = howToWorkDefault[2],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[3],
                EnglishLevel = englishLevelsDefault[3],
                TypeOfCompany = typeOfCompanyDefault[1],
                EmployerUser = employerUser2,
                FromSalary = 1500,
                ToSalary = 2000,
                ExperienceWork = 1,
                KeyWords = "Python, Software, Engineer"
            },
            new()
            {
                VacancyTitle = "Middle UX/UI Designer",
                ShortDescription =
                    "We are in a need of a skilled designer, who is a proactive and enthusiastic personality. If you strive for new challenges - let's work on an interesting and valuable project.  ",
                LongDescription =
                    "<p>Requirements:</p><p>- 2+ years of experience in UX/UI design for web and mobile platforms;</p><p>- Strong portfolio of work that demonstrates your design process and problem-solving skills;</p><p>Strong knowledge of components and styles in Figma;</p><p>- Pixel perfect design approach;</p><p>- Knowledge of Android and iOS guidelines;</p><p>- Comfortable in creating and working with design systems;</p><p>- Understanding of responsive web design and accessible web design, designing for different screens and devices;</p><p>- Experience in prototyping and interaction design;</p><p><br></p><p>Tasks:</p><p>- Design innovative user experiences for our products and complement them with an intuitive, cohesive look &amp; feel (UI);</p><p>- Build interfaces and experiences for mobile/web products in Fintech Domain;</p><p>- Conduct competitors research;</p><p>- Participate in the project development lifecycle;</p><p>- Interact with the team to share and discuss design ideas (PM, BA, and development teams);</p><p>- Development of marketing products: landing pages, project cases, banners, blog support.</p><p><br></p><p>We offer:</p><p>- Possibility to work either in the office or remotely when needed;</p><p>- 18 paid working days of vacation;</p><p>- 10 paid sick leaves without documents/year;</p><p>- Corporate events and team buildings;</p><p>- Self- improvement and professional growth with a team.</p><p><br></p>",
                VacancyDomain = vacancyDomainDefault[12],
                WorkCategory = workCategoriesDefault[17],
                HowToWork = howToWorkDefault[0],
                Relocate = relocateDefault[2],
                CandidateRegion = candidateRegionsDefault[2],
                EnglishLevel = englishLevelsDefault[2],
                TypeOfCompany = typeOfCompanyDefault[0],
                EmployerUser = employerUser,
                FromSalary = 2000,
                ToSalary = 4000,
                ExperienceWork = 3,
                KeyWords = "Middle, UX/UI, Designer"
            },
        };

        if (!await _context.Vacancies.AnyAsync())
        {
            await _context.AddRangeAsync(vacanciesDefault);
            await _context.SaveChangesAsync();
        }
    }
}