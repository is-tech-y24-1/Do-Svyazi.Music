using DS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.DataAccess.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasOne<PlaylistSongs>("_songs");
        builder.Ignore(x => x.Songs);
        builder.HasOne(s => s.Author);
    }
}