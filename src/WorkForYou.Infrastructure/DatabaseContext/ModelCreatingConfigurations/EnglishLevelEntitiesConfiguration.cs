using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class EnglishLevelEntitiesConfiguration : IEntityTypeConfiguration<EnglishLevel>
{
    public void Configure(EntityTypeBuilder<EnglishLevel> builder)
    {
        builder.HasMany(x => x.CandidateUsers)
            .WithOne(x => x.LevelEnglish)
            .HasForeignKey(x => x.EnglishLevelId);
        
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.EnglishLevel)
            .HasForeignKey(x => x.EnglishLevelId);
    }
}
