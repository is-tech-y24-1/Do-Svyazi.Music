using DS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.DataAccess.Configurations;

public class PlaylistSongNodeConfiguration : IEntityTypeConfiguration<PlaylistSongNode>
{
    public void Configure(EntityTypeBuilder<PlaylistSongNode> builder)
    {
        builder.HasOne(n => n.NextSongNode);
        builder.HasOne(n => n.Song);
    }
}