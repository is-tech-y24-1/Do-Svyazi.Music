using DS.DataAccess.Seeding.Generators;
using DS.Domain;
using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess.Seeding;

public class AutoBogusSeeder : IDbContextSeeder
{
    private const int MusicUsersCount = 50;
    private const int GenresCount = 10;
    private const int SongsCount = 100;
    private const int PlaylistsCount = 100;
    
    public void Seed(ModelBuilder modelBuilder)
    {
        ICollection<SongGenre> genres = GenreGenerator.GenerateSongGenres(GenresCount);
        ICollection<MusicUser> users = MusicUserGenerator.GenerateMusicUsers(MusicUsersCount);
        ICollection<Song> songs = SongGenerator.GenerateSongs(users, genres, SongsCount);
        ICollection<Playlist> playlists = PlaylistGenerator.GeneratePlaylists(songs, users, PlaylistsCount);
        
        MediaLibraryFiller.FillMediaLibraries(users, playlists, songs);
        ListeningQueueFiller.FillListeningQueues(users, songs);

        modelBuilder.Entity<MusicUser>().HasData(users);
        modelBuilder.Entity<SongGenre>().HasData(genres);
        modelBuilder.Entity<Song>().HasData(songs);
        modelBuilder.Entity<Playlist>().HasData(playlists);
    }
}