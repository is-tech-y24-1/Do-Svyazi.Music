using DS.DataAccess.Context;
using DS.DataAccess.Seeding.Generators;
using DS.Domain;

namespace DS.DataAccess.Seeding;

public class AutoBogusSeeder : IDbContextSeeder
{
    private const int MusicUsersCount = 50;
    private const int GenresCount = 10;
    private const int SongsCount = 100;
    private const int PlaylistsCount = 100;

    public void Seed(MusicDbContext context)
    {
        ICollection<SongGenre> genres = GenreGenerator.GenerateSongGenres(GenresCount);
        ICollection<MusicUser> users = MusicUserGenerator.GenerateMusicUsers(MusicUsersCount);
        ICollection<Song> songs = SongGenerator.GenerateSongs(users, genres, SongsCount);
        ICollection<Playlist> playlists = PlaylistGenerator.GeneratePlaylists(songs, users, PlaylistsCount);
        
        MediaLibraryFiller.FillMediaLibraries(users, playlists, songs);
        ListeningQueueFiller.FillListeningQueues(users, songs);
    }
}