using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class CommunicationEntitiesConfiguration : IEntityTypeConfiguration<CommunicationLanguage>
{
    public void Configure(EntityTypeBuilder<CommunicationLanguage> builder)
    {
        builder.HasMany(x => x.CandidateUsers)
            .WithOne(x => x.CommunicationLanguage)
            .HasForeignKey(x => x.CommunicationLanguageId);
    }
}
