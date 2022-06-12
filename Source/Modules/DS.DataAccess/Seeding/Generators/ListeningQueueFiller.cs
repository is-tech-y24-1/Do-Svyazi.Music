using Bogus;
using DS.Common.Extensions;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public class ListeningQueueFiller
{
    private const int MinContent = 0;
    private const int MaxContent = 20;

    public void FillListeningQueues(ICollection<MusicUser> users, ICollection<Song> songs)
    {
        users.ThrowIfNull();
        songs.ThrowIfNull();

        var faker = new Faker();
        
        foreach (MusicUser musicUser in users)
        {
            PlaylistSongs content = GeneratePlaylistSongs(songs, faker);

            foreach (Song playlistSong in content)
                musicUser.ListeningQueue.AddLastToPlay(playlistSong);
        }
    }
    
    private PlaylistSongs GeneratePlaylistSongs(ICollection<Song> songs, Faker faker)
    {
        int n = faker.Random.Int(MinContent, MaxContent);
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