using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class ChatUserEntitiesConfiguration : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.HasKey(x => new {x.ApplicationUserId, x.ChatRoomId});

        builder.HasOne(x => x.ApplicationUser)
            .WithMany(x => x.ChatUsers)
            .HasForeignKey(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ChatRoom)
            .WithMany(x => x.ChatUsers)
            .HasForeignKey(x => x.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
