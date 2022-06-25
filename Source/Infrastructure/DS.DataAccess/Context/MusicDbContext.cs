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
        ConfigureMusicUser(modelBuilder);
        ConfigureSong(modelBuilder);
        ConfigureSongGenre(modelBuilder);
        ConfigurePlaylistSongNode(modelBuilder);
        ConfigurePlaylistSongs(modelBuilder);
        ConfigurePlaylist(modelBuilder);
        ConfigureListeningQueue(modelBuilder);
        ConfigureMediaLibrary(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureMusicUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MusicUser>().Property(mu => mu.Id).ValueGeneratedNever();
    }

    private static void ConfigureSong(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>().Property(s => s.Id).ValueGeneratedNever();
        modelBuilder.Entity<Song>()
            .HasMany("_featuring")
            .WithMany("_featuredSongs");
    }

    private static void ConfigureSongGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SongGenre>().Property(sg => sg.Id).ValueGeneratedNever();
    }

    private static void ConfigurePlaylistSongNode(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaylistSongNode>().Property(psn => psn.Id).ValueGeneratedNever();
    }

    private static void ConfigurePlaylistSongs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaylistSongs>().Property(ps => ps.Id).ValueGeneratedNever();
        modelBuilder.Entity<PlaylistSongs>().HasOne("_head");
        modelBuilder.Entity<PlaylistSongs>().HasOne("_tail");
    }

    private static void ConfigurePlaylist(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Playlist>().Property(p => p.Id).ValueGeneratedNever();
        modelBuilder.Entity<Playlist>().HasOne("_songs");
        modelBuilder.Entity<Playlist>().Ignore(p => p.Songs);
    }

    private static void ConfigureListeningQueue(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ListeningQueue>().HasKey(lq => lq.OwnerId);
        modelBuilder.Entity<ListeningQueue>().Property(lq => lq.OwnerId).ValueGeneratedNever();
        modelBuilder.Entity<ListeningQueue>().HasOne("_songs");
        modelBuilder.Entity<ListeningQueue>().Ignore(lq => lq.Songs);
    }

    private static void ConfigureMediaLibrary(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MediaLibrary>().HasKey(ml => ml.OwnerId);
        modelBuilder.Entity<MediaLibrary>().Property(ml => ml.OwnerId).ValueGeneratedNever();
        modelBuilder.Entity<MediaLibrary>().Ignore(ml => ml.Playlists);
        modelBuilder.Entity<MediaLibrary>().Ignore(ml => ml.AuthoredPlaylists);
        modelBuilder.Entity<MediaLibrary>().Ignore(ml => ml.Songs);
        modelBuilder.Entity<MediaLibrary>().Ignore(ml => ml.AuthoredSongs);
        modelBuilder.Entity<MediaLibrary>()
            .HasMany("_songs")
            .WithMany("_addedToLibraries");
        
        modelBuilder.Entity<MediaLibrary>()
            .HasMany("_playlists")
            .WithMany("_addedToLibraries");
    }
}