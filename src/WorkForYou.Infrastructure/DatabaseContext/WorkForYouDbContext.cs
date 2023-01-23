using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkForYou.Data.Models;
using WorkForYou.Data.Models.IdentityInheritance;

namespace WorkForYou.Infrastructure.DatabaseContext;

public class WorkForYouDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public WorkForYouDbContext(DbContextOptions<WorkForYouDbContext> options) 
        :base(options) {}
    
    public DbSet<CandidateUser> CandidateUsers => Set<CandidateUser>();
    public DbSet<EmployerUser> EmployerUsers => Set<EmployerUser>();
    public DbSet<EnglishLevel> EnglishLevels => Set<EnglishLevel>();
    public DbSet<WorkCategory> WorkCategories => Set<WorkCategory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CandidateUser>()
            .Property(x => x.CandidateUserId).ValueGeneratedOnAdd();

        builder.Entity<EmployerUser>()
            .Property(x => x.EmployerUserId).ValueGeneratedOnAdd();

        builder.Entity<ApplicationUser>()
            .HasOne(x => x.CandidateUser)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey<CandidateUser>(x => x.ApplicationUserId);

        builder.Entity<ApplicationUser>()
            .HasOne(x => x.EmployerUser)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey<EmployerUser>(x => x.ApplicationUserId);

        builder.Entity<EnglishLevel>()
            .HasKey(x => x.EnglishLevelId);

        builder.Entity<EnglishLevel>()
            .HasOne(x => x.User)
            .WithOne(x => x.LevelEnglish)
            .HasForeignKey<CandidateUser>(x => x.EnglishLevelId);

        builder.Entity<WorkCategory>()
            .HasKey(x => x.WorkCategoryId);
        
        builder.Entity<WorkCategory>()
            .HasOne(x => x.User)
            .WithOne(x => x.CategoryWork)
            .HasForeignKey<CandidateUser>(x => x.WorkCategoryId);
    }
}
