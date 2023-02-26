using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class RelocateEntitiesConfiguration : IEntityTypeConfiguration<Relocate>
{
    public void Configure(EntityTypeBuilder<Relocate> builder)
    {
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.Relocate)
            .HasForeignKey(x => x.RelocateId);
    }
}
