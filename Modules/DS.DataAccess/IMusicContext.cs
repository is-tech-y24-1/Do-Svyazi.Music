using DS.Domain;
using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess;

public interface IMusicContext
{
    public DbSet<ListeningQueue> ListeningQueues { get; }
    public DbSet<MediaLibrary> MediaLibraries { get; }
    public DbSet<MusicUser> MusicUsers { get; }
    public DbSet<Playlist> Playlists { get; }
    public DbSet<PlaylistSongNode> PlaylistSongNodes { get; }
    public DbSet<PlaylistSongs> PlaylistSongs { get; }
    public DbSet<Song> Songs { get; }
    public DbSet<SongGenre> SongGenres { get; }
}