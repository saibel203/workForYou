﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class UserEntitiesConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(x => x.CandidateUser)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey<CandidateUser>(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.EmployerUser)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey<EmployerUser>(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
