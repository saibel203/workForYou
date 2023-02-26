using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class EmployerEntitiesConfiguration : IEntityTypeConfiguration<EmployerUser>
{
    public void Configure(EntityTypeBuilder<EmployerUser> builder)
    {
        builder.HasMany(x => x.Vacancies)
            .WithOne(x => x.EmployerUser)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.ChatRooms)
            .WithOne(x => x.EmployerUser);
    }
}
