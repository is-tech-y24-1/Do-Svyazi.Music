using Bogus;
using DS.Common.Extensions;
using DS.DataAccess.Seeding.Fakers;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public static class SongGenerator
{
    private const int MinFeaturing = 0;
    private const int MaxFeaturing = 2;

    public static ICollection<Song> GenerateSongs(ICollection<MusicUser> authors, ICollection<SongGenre> genres, int count)
    {
        authors.ThrowIfNull();
        genres.ThrowIfNull();
        
        var songs = new List<Song>();
        var faker = new Faker();

        for (int i = 0; i < count; i++)
        {
            MusicUser author = faker.PickRandom(authors);
            SongGenre genre = faker.PickRandom(genres);
            Song song = new SongFaker(author, genre).Generate();
            
            AddFeaturing(song, authors, faker);
            author.MediaLibrary.AddSong(song);
            songs.Add(song);
        }

        return songs;
    }

    private static void AddFeaturing(Song song, ICollection<MusicUser> users, Faker faker)
    {
        int n = faker.Random.Int(MinFeaturing, MaxFeaturing);

        for (int i = 0; i < n; i++)
        {
            MusicUser feat = faker.PickRandom(users);
            if (song.Featuring.Contains(feat) || song.Author.Equals(feat))
                continue;
            
            song.AddFeaturingUser(feat);
        }
    }
}