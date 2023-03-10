using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class RespondedListEntitiesConfiguration : IEntityTypeConfiguration<RespondedList>
{
    public void Configure(EntityTypeBuilder<RespondedList> builder)
    {
        builder.HasKey(x => new {x.ApplicationUserId, x.VacancyId});

        builder
            .HasOne(x => x.ApplicationUser)
            .WithMany(x => x.RespondedList)
            .HasForeignKey(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Vacancy)
            .WithMany(x => x.RespondedList)
            .HasForeignKey(x => x.VacancyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
