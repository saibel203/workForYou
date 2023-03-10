using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class RegionEntitiesConfiguration : IEntityTypeConfiguration<CandidateRegion>
{
    public void Configure(EntityTypeBuilder<CandidateRegion> builder)
    {
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.CandidateRegion)
            .HasForeignKey(x => x.CandidateRegionId);
    }
}
