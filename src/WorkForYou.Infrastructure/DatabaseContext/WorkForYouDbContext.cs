using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Infrastructure.DatabaseContext;

public class WorkForYouDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public WorkForYouDbContext(DbContextOptions<WorkForYouDbContext> options)
        : base(options)
    {
    }

    public DbSet<CandidateUser> CandidateUsers => Set<CandidateUser>();
    public DbSet<EmployerUser> EmployerUsers => Set<EmployerUser>();
    public DbSet<EnglishLevel> EnglishLevels => Set<EnglishLevel>();
    public DbSet<WorkCategory> WorkCategories => Set<WorkCategory>();
    public DbSet<HowToWork> HowToWorks => Set<HowToWork>();
    public DbSet<Relocate> Relocates => Set<Relocate>();
    public DbSet<CandidateRegion> CandidateRegions => Set<CandidateRegion>();
    public DbSet<TypesOfCompany> TypesOfCompany => Set<TypesOfCompany>();
    public DbSet<VacancyDomain> VacancyDomains => Set<VacancyDomain>();
    public DbSet<Vacancy> Vacancies => Set<Vacancy>();
    public DbSet<FavouriteVacancy> FavouriteVacancies => Set<FavouriteVacancy>();
    public DbSet<FavouriteCandidate> FavouriteCandidates => Set<FavouriteCandidate>();
    public DbSet<CommunicationLanguage> CommunicationLanguages => Set<CommunicationLanguage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasOne(x => x.CandidateUser)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey<CandidateUser>(x => x.ApplicationUserId);

        builder.Entity<ApplicationUser>()
            .HasOne(x => x.EmployerUser)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey<EmployerUser>(x => x.ApplicationUserId);

        builder.Entity<CommunicationLanguage>()
            .HasOne(x => x.User)
            .WithOne(x => x.CommunicationLanguage)
            .HasForeignKey<CandidateUser>(x => x.CommunicationLanguageId);

        builder.Entity<EnglishLevel>()
            .HasOne(x => x.User)
            .WithOne(x => x.LevelEnglish)
            .HasForeignKey<CandidateUser>(x => x.EnglishLevelId);

        builder.Entity<WorkCategory>()
            .HasOne(x => x.User)
            .WithOne(x => x.CategoryWork)
            .HasForeignKey<CandidateUser>(x => x.WorkCategoryId);

        builder.Entity<HowToWork>()
            .HasOne(x => x.Vacancy)
            .WithOne(x => x.HowToWork)
            .HasForeignKey<Vacancy>(x => x.HowToWorkId);

        builder.Entity<Relocate>()
            .HasOne(x => x.Vacancy)
            .WithOne(x => x.Relocate)
            .HasForeignKey<Vacancy>(x => x.RelocateId);

        builder.Entity<CandidateRegion>()
            .HasOne(x => x.Vacancy)
            .WithOne(x => x.CandidateRegion)
            .HasForeignKey<Vacancy>(x => x.CandidateRegionId);

        builder.Entity<TypesOfCompany>()
            .HasOne(x => x.Vacancy)
            .WithOne(x => x.TypeOfCompany)
            .HasForeignKey<Vacancy>(x => x.TypesOfCompanyId);

        builder.Entity<VacancyDomain>()
            .HasOne(x => x.Vacancy)
            .WithOne(x => x.VacancyDomain)
            .HasForeignKey<Vacancy>(x => x.VacancyDomainId);

        builder.Entity<EmployerUser>()
            .HasMany(x => x.Vacancies)
            .WithOne(x => x.EmployerUser)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<FavouriteVacancy>()
            .HasKey(x => new {x.CandidateId, x.VacancyId});

        builder.Entity<FavouriteVacancy>()
            .HasOne(x => x.Vacancy)
            .WithMany(x => x.FavouriteVacancyCollection)
            .HasForeignKey(x => x.VacancyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<FavouriteVacancy>()
            .HasOne(x => x.CandidateUser)
            .WithMany(x => x.FavouriteVacancyCollection)
            .HasForeignKey(x => x.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<FavouriteCandidate>()
            .HasKey(x => new {x.CandidateUserId, x.EmployerUserId});

        builder.Entity<FavouriteCandidate>()
            .HasOne(x => x.EmployerUser)
            .WithMany(x => x.FavouriteCandidates)
            .HasForeignKey(x => x.EmployerUserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<FavouriteCandidate>()
            .HasOne(x => x.CandidateUser)
            .WithMany(x => x.FavouriteCandidates)
            .HasForeignKey(x => x.CandidateUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}