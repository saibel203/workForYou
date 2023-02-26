using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class CompanyTypesEntitiesConfiguration : IEntityTypeConfiguration<TypesOfCompany>
{
    public void Configure(EntityTypeBuilder<TypesOfCompany> builder)
    {
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.TypeOfCompany)
            .HasForeignKey(x => x.TypesOfCompanyId);
    }
}
