using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class WorkCategoryEntitiesConfiguration : IEntityTypeConfiguration<WorkCategory>
{
    public void Configure(EntityTypeBuilder<WorkCategory> builder)
    {
        builder.HasMany(x => x.CandidateUsers)
            .WithOne(x => x.CategoryWork)
            .HasForeignKey(x => x.WorkCategoryId);

        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.WorkCategory)
            .HasForeignKey(x => x.WorkCategoryId);
    }
}
