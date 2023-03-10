using Microsoft.EntityFrameworkCore;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.SharedForTests;

public static class WorkForYouDbContextFactory
{
    public static WorkForYouDbContext CreateEmpty()
    {
        DbContextOptions<WorkForYouDbContext> options = new DbContextOptionsBuilder<WorkForYouDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        WorkForYouDbContext context = new WorkForYouDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }
    
    public static WorkForYouDbContext CreateWithData()
    {
        DbContextOptions<WorkForYouDbContext> options = new DbContextOptionsBuilder<WorkForYouDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        WorkForYouDbContext context = new WorkForYouDbContext(options);

        context.Database.EnsureCreated();

        IEnumerable<CandidateRegion> candidateRegions = new List<CandidateRegion>
        {
            new() {CandidateRegionName = "Name 1"}
        };

        context.CandidateRegions.AddRange(candidateRegions);
        context.SaveChanges();

        IEnumerable<EnglishLevel> englishLevels = new List<EnglishLevel>
        {
            new() {NameLevel = "Level 1"}
        };

        context.EnglishLevels.AddRange(englishLevels);
        context.SaveChanges();

        IEnumerable<CommunicationLanguage> communicationLanguages = new List<CommunicationLanguage>
        {
            new() {CommunicationLanguageName = "Language 1"}
        };

        context.CommunicationLanguages.AddRange(communicationLanguages);
        context.SaveChanges();
        
        IEnumerable<HowToWork> howToWorks = new List<HowToWork>
        {
            new() {HowToWorkName = "HowToWork 1"}
        };

        context.HowToWorks.AddRange(howToWorks);
        context.SaveChanges();
        
        IEnumerable<Relocate> relocates = new List<Relocate>
        {
            new() {RelocateName = "Relocate 1"}
        };

        context.Relocates.AddRange(relocates);
        context.SaveChanges();
        
        IEnumerable<TypesOfCompany> typesOfCompanies = new List<TypesOfCompany>
        {
            new() {TypeOfCompanyName = "TypeOfCompany 1"}
        };

        context.TypesOfCompany.AddRange(typesOfCompanies);
        context.SaveChanges();
        
        IEnumerable<VacancyDomain> vacancyDomains = new List<VacancyDomain>
        {
            new() {VacancyDomainName = "VacancyDomain 1"}
        };

        context.VacancyDomains.AddRange(vacancyDomains);
        context.SaveChanges();
        
        IEnumerable<WorkCategory> workCategories = new List<WorkCategory>
        {
            new() {CategoryName = "WorkCategory 1"}
        };

        context.WorkCategories.AddRange(workCategories);
        context.SaveChanges();

        EmployerUser employerUser = new EmployerUser
        {
            EmployerUserId = 1,
        };

        CandidateUser candidateUser = new CandidateUser
        {
            CandidateUserId = 1,
        };

        IEnumerable<ApplicationUser> applicationUsers = new List<ApplicationUser>
        {
            new()
            {
                UserName = "username 1",
                Email = "username1@gmail.com",
                Id = Guid.Parse("3f8a165e-102a-4124-a485-dcfa7daa24fc").ToString(),
                EmployerUser = employerUser
            },
            new()
            {
                UserName = "username 2",
                Email = "username2@gmail.com",
                Id = Guid.Parse("721395f9-0a14-4cfd-880b-d9e90d2f2b0b").ToString(),
                CandidateUser = candidateUser
            }
        };
        
        context.Users.AddRange(applicationUsers);
        context.SaveChanges();

        // IEnumerable<Vacancy> vacancies = new List<Vacancy>
        // {
        //     new()
        //     {
        //         VacancyTitle = "title",
        //         ShortDescription = "short",
        //         LongDescription = "long",
        //         FromSalary = 1,
        //         ToSalary = 2,
        //         ExperienceWork = 1,
        //         KeyWords = "keywords",
        //         WorkCategoryId = 1,
        //         VacancyDomainId = 1,
        //         HowToWorkId = 1,
        //         RelocateId = 1,
        //         CandidateRegionId = 1,
        //         EnglishLevelId = 1,
        //         TypesOfCompanyId = 1,
        //         EmployerUser = employerUser
        //     },
        //     new()
        //     {
        //         VacancyTitle = "title",
        //         ShortDescription = "short",
        //         LongDescription = "long",
        //         FromSalary = 1,
        //         ToSalary = 2,
        //         ExperienceWork = 1,
        //         KeyWords = "keywords",
        //         WorkCategoryId = 1,
        //         VacancyDomainId = 1,
        //         HowToWorkId = 1,
        //         RelocateId = 1,
        //         CandidateRegionId = 1,
        //         EnglishLevelId = 1,
        //         TypesOfCompanyId = 1,
        //         EmployerUser = employerUser
        //     }
        // };
        //
        // context.Vacancies.AddRange(vacancies);
        // context.SaveChanges();

        return context;
    }

    public static void Destroy(WorkForYouDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}