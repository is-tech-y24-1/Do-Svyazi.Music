using DS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.DataAccess.Configurations;

public class ListeningQueueConfiguration : IEntityTypeConfiguration<ListeningQueue>
{
    public void Configure(EntityTypeBuilder<ListeningQueue> builder)
    {
        builder.HasKey(q => q.OwnerId);
        builder.Navigation(q => q.Songs)
            .HasField("_songs");
    }
}