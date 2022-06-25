using DS.Domain;
using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess;

public interface IMusicContext
{
    DbSet<ListeningQueue> ListeningQueues { get; }
    DbSet<MediaLibrary> MediaLibraries { get; }
    DbSet<MusicUser> MusicUsers { get; }
    DbSet<Playlist> Playlists { get; }
    DbSet<PlaylistSongNode> PlaylistSongNodes { get; }
    DbSet<PlaylistSongs> PlaylistSongs { get; }
    DbSet<Song> Songs { get; }
    DbSet<SongGenre> SongGenres { get; }
}