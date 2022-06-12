using Bogus;
using DS.Common.Extensions;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public class MediaLibraryFiller
{
    private const int MinContent = 0;
    private const int MaxContent = 20;

    public static void FillMediaLibraries(ICollection<MusicUser> users, ICollection<Playlist> playlists, ICollection<Song> songs)
    {
        users.ThrowIfNull();
        playlists.ThrowIfNull();
        songs.ThrowIfNull();

        var faker = new Faker();
        
        foreach (MusicUser musicUser in users)
        {
            FillWithPlaylists(musicUser, playlists, faker);
            FillWithSongs(musicUser, songs, faker);
        }
    }

    private static void FillWithSongs(MusicUser musicUser, ICollection<Song> songs, Faker faker)
    {
        int n = faker.Random.Int(MinContent, MaxContent);

        for (int i = 0; i < n; i++)
        {
            Song song = faker.PickRandom(songs);

            if (musicUser.MediaLibrary.Songs.Contains(song))
                continue;
            
            musicUser.MediaLibrary.AddSong(song);
        }
    }

    private static void FillWithPlaylists(MusicUser musicUser, ICollection<Playlist> playlists, Faker faker)
    {
        int n = faker.Random.Int(MinContent, MaxContent);

        for (int i = 0; i < n; i++)
        {
            Playlist playlist = faker.PickRandom(playlists);

            if (musicUser.MediaLibrary.Playlists.Contains(playlist))
                continue;
            
            musicUser.MediaLibrary.AddPlaylist(playlist);
        }
    }
}