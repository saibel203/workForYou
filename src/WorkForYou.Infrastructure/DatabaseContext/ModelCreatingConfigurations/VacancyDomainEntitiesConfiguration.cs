using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class VacancyDomainEntitiesConfiguration : IEntityTypeConfiguration<VacancyDomain>
{
    public void Configure(EntityTypeBuilder<VacancyDomain> builder)
    {
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.VacancyDomain)
            .HasForeignKey(x => x.VacancyDomainId);
    }
}
