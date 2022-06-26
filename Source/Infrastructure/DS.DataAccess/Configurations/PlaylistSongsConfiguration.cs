using DS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.DataAccess.Configurations;

public class PlaylistSongsConfiguration : IEntityTypeConfiguration<PlaylistSongs>
{
    public void Configure(EntityTypeBuilder<PlaylistSongs> builder)
    {
        builder.HasOne<PlaylistSongNode>("_head");
        builder.HasOne<PlaylistSongNode>("_tail");
    }
}