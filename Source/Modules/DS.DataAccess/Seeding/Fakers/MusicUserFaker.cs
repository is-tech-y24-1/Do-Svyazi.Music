using AutoBogus;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class MusicUserFaker : AutoFaker<MusicUser>
{
    public static readonly MusicUserFaker Instance = new MusicUserFaker();

    private MusicUserFaker()
    {
        var id = Guid.NewGuid();
        RuleFor(e => e.Id, _ => id);
        RuleFor(e => e.Name, f => f.Internet.UserName());
        RuleFor(e => e.ProfilePictureUri, f => f.Internet.Avatar());
        RuleFor(e => e.MediaLibrary, _ => new MediaLibrary(id));
        RuleFor(e => e.ListeningQueue, _ => new ListeningQueue(id));
    }
}