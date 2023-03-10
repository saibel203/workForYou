using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class FavouriteVacancyEntitiesConfiguration : IEntityTypeConfiguration<FavouriteVacancy>
{
    public void Configure(EntityTypeBuilder<FavouriteVacancy> builder)
    {
        builder.HasKey(x => new {x.CandidateId, x.VacancyId});

        builder.HasOne(x => x.Vacancy)
            .WithMany(x => x.FavouriteVacancyCollection)
            .HasForeignKey(x => x.VacancyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CandidateUser)
            .WithMany(x => x.FavouriteVacancyCollection)
            .HasForeignKey(x => x.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
