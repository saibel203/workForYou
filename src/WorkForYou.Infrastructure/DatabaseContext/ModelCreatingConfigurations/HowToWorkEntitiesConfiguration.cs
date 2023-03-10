using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class HowToWorkEntitiesConfiguration : IEntityTypeConfiguration<HowToWork>
{
    public void Configure(EntityTypeBuilder<HowToWork> builder)
    {
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.HowToWork)
            .HasForeignKey(x => x.HowToWorkId);
    }
}
