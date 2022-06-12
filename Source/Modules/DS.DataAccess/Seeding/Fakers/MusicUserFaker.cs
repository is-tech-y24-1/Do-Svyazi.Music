using AutoBogus;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class MusicUserFaker : AutoFaker<MusicUser>
{
    public MusicUserFaker()
    {
        var id = Guid.NewGuid();
        RuleFor(musicUser => musicUser.Id, id);
        RuleFor(musicUser => musicUser.Name, faker => faker.Internet.UserName());
        RuleFor(musicUser => musicUser.ProfilePictureUri, faker => faker.Internet.Avatar());
        RuleFor(musicUser => musicUser.MediaLibrary, new MediaLibrary(id));
        RuleFor(musicUser => musicUser.ListeningQueue, new ListeningQueue(id));
    }
}