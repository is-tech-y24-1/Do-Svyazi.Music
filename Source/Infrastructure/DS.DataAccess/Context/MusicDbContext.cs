using DS.Domain;
using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess.Context;

public sealed class MusicDbContext : DbContext, IMusicContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<ListeningQueue> ListeningQueues { get; private set; } = null!;
    public DbSet<MediaLibrary> MediaLibraries { get; private set; } = null!;
    public DbSet<MusicUser> MusicUsers { get; private set; } = null!;
    public DbSet<Playlist> Playlists { get; private set; } = null!;
    public DbSet<PlaylistSongNode> PlaylistSongNodes { get; private set; } = null!;
    public DbSet<PlaylistSongs> PlaylistSongs { get; private set; } = null!;
    public DbSet<Song> Songs { get; private set; } = null!;
    public DbSet<SongGenre> SongGenres { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicDbContext).Assembly);
    }
}