using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkForYou.Core.Models;

namespace WorkForYou.Infrastructure.DatabaseContext.ModelCreatingConfigurations;

public class ChatRoomEntitiesConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasKey(x => x.ChatRoomId);

        builder.HasMany(x => x.ChatMessages)
            .WithOne(x => x.ChatRoom);

        builder.HasMany(x => x.ChatUsers)
            .WithOne(x => x.ChatRoom);
    }
}
