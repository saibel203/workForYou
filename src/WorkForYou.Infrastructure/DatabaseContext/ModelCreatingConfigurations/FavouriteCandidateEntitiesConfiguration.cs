using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class FavouriteCandidateEntitiesConfiguration : IEntityTypeConfiguration<FavouriteCandidate>
{
    public void Configure(EntityTypeBuilder<FavouriteCandidate> builder)
    {
        builder.HasKey(x => new {x.CandidateUserId, x.EmployerUserId});

        builder.HasOne(x => x.EmployerUser)
            .WithMany(x => x.FavouriteCandidates)
            .HasForeignKey(x => x.EmployerUserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.CandidateUser)
            .WithMany(x => x.FavouriteCandidates)
            .HasForeignKey(x => x.CandidateUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
