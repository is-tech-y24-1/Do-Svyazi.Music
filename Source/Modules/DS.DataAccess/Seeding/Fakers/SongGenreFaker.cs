using AutoBogus;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class SongGenreFaker : AutoFaker<SongGenre>
{
    public SongGenreFaker()
    {
        RuleFor(e => e.Id, Guid.NewGuid);
        RuleFor(e => e.Name, f => f.Music.Genre());
    }
}