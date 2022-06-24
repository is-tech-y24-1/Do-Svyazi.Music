using DS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.DataAccess.Configurations;

public class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.HasOne(s => s.Genre);
        builder.HasOne(s => s.Author);

        builder.Navigation(s => s.Featuring)
            .HasField("_featuring");
    }
}