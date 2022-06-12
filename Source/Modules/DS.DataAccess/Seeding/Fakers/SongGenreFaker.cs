using AutoBogus;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class SongGenreFaker : AutoFaker<SongGenre>
{
    public SongGenreFaker()
    {
        RuleFor(songGenre => songGenre.Id, Guid.NewGuid);
        RuleFor(songGenre => songGenre.Name, faker => faker.Music.Genre());
    }
}