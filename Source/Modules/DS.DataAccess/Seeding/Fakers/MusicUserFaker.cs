using AutoBogus;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class MusicUserFaker : AutoFaker<MusicUser>
{
    public MusicUserFaker()
    {
        var id = Guid.NewGuid();
        RuleFor(e => e.Id, id);
        RuleFor(e => e.Name, f => f.Internet.UserName());
        RuleFor(e => e.ProfilePictureUri, f => f.Internet.Avatar());
        RuleFor(e => e.MediaLibrary, new MediaLibrary(id));
        RuleFor(e => e.ListeningQueue, new ListeningQueue(id));
    }
}