using DS.DataAccess.Seeding.Fakers;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public class GenreGenerator
{
    public IReadOnlyCollection<SongGenre> GenerateSongGenres(int count)
    {
        return new SongGenreFaker().Generate(count);
    }
}