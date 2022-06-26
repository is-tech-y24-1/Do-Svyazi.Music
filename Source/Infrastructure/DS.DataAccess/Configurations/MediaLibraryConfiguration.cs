using DS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.DataAccess.Configurations;

public class MediaLibraryConfiguration : IEntityTypeConfiguration<MediaLibrary>
{
    public void Configure(EntityTypeBuilder<MediaLibrary> builder)
    {
        builder.HasKey(l => l.OwnerId);
        builder.Navigation(l => l.Songs).HasField("_songs");
        builder.Ignore(l => l.AuthoredSongs);
        builder.Navigation(l => l.Playlists).HasField("_playlists");
        builder.Ignore(l => l.AuthoredPlaylists);
    }
}