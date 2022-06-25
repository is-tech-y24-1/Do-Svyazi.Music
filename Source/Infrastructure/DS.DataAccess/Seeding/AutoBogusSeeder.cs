using DS.Common.Extensions;
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
        context.ThrowIfNull();
        
        ICollection<SongGenre> genres = GenreGenerator.GenerateSongGenres(GenresCount);
        foreach (SongGenre songGenre in genres)
            context.Add(songGenre);
        context.SaveChanges();

        ICollection<MusicUser> users = MusicUserGenerator.GenerateMusicUsers(MusicUsersCount);
        foreach (MusicUser musicUser in users)
            context.Add(musicUser);
        context.SaveChanges();

        ICollection<Song> songs = SongGenerator.GenerateSongs(users, genres, SongsCount);
        foreach (Song song in  songs)
            context.Add(song);
        context.SaveChanges();
        
        ICollection<Playlist> playlists = PlaylistGenerator.GeneratePlaylists(songs, users, PlaylistsCount);
        foreach (Playlist playlist in playlists)
            context.Add(playlist);
        context.SaveChanges();
            

        MediaLibraryFiller.FillMediaLibraries(users, playlists, songs);
        ListeningQueueFiller.FillListeningQueues(users, songs);
        context.SaveChanges();
    }
}