using Bogus;
using DS.Common.Extensions;
using DS.DataAccess.Seeding.Fakers;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public class PlaylistGenerator
{
    private const int SongsMin = 2;
    private const int SongsMax = 20;

    public IReadOnlyCollection<Playlist> GeneratePlaylists(
        ICollection<Song> songs,
        ICollection<MusicUser> authors,
        int count)
    {
        songs.ThrowIfNull();
        authors.ThrowIfNull();
        
        var faker = new Faker();
        var playlists = new List<Playlist>();

        for (int i = 0; i < count; i++)
        {
            MusicUser author = faker.PickRandom(authors);
            PlaylistSongs content = GeneratePlaylistSongs(songs, faker);

            Playlist playlist = new PlaylistFaker(author, content).Generate();
            author.MediaLibrary.AddPlaylist(playlist);
            playlists.Add(playlist);
        }

        return playlists;
    }

    private PlaylistSongs GeneratePlaylistSongs(ICollection<Song> songs, Faker faker)
    {
        int n = faker.Random.Int(SongsMin, SongsMax);
        var playlistSongs = new PlaylistSongs();

        for (int i = 0; i < n; i++)
        {
            Song song = faker.PickRandom(songs);
            if (playlistSongs.Contains(song))
                continue;
            
            playlistSongs.Add(song);
        }

        return playlistSongs;
    }
}