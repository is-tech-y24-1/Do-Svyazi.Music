using DS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.DataAccess.Configurations;

public class MusicUserConfiguration : IEntityTypeConfiguration<MusicUser>
{
    public void Configure(EntityTypeBuilder<MusicUser> builder)
    {
        builder.HasOne(u => u.MediaLibrary);
        builder.HasOne(u => u.ListeningQueue);
    }
}