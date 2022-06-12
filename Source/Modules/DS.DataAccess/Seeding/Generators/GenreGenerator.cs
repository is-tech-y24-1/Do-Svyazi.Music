using DS.DataAccess.Seeding.Fakers;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public class GenreGenerator
{
    public static ICollection<SongGenre> GenerateSongGenres(int count) => new SongGenreFaker().Generate(count);
}