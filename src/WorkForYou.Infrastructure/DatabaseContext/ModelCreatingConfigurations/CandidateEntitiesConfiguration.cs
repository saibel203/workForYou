using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class CandidateEntitiesConfiguration : IEntityTypeConfiguration<CandidateUser>
{
    public void Configure(EntityTypeBuilder<CandidateUser> builder)
    {
    }
}
