using DS.DataAccess.Seeding.Fakers;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public class MusicUserGenerator
{
    public static ICollection<MusicUser> GenerateMusicUsers(int count)
    {
        var users = new List<MusicUser>();

        for (int i = 0; i < count; i++)
        {
            users.Add(new MusicUserFaker().Generate());
        }

        return users;
    }
}