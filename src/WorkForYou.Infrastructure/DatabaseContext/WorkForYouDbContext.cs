using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

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
    public DbSet<RespondedList> RespondedList => Set<RespondedList>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
    public DbSet<ChatUser> ChatUsers => Set<ChatUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(UserEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(CommunicationEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(EnglishLevelEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(WorkCategoryEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(HowToWorkEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(RelocateEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(RegionEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(CompanyTypesEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(CompanyTypesEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(FavouriteVacancyEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(FavouriteCandidateEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(RespondedListEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(CandidateEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(EmployerEntitiesConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(ChatRoomEntitiesConfiguration).Assembly);
    }
}