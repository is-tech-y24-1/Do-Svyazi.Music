﻿using DS.DataAccess.Seeding.Fakers;
using DS.Domain;

namespace DS.DataAccess.Seeding.Generators;

public class MusicUserGenerator
{
    public static ICollection<MusicUser> GenerateMusicUsers(int count)
    {
        return new MusicUserFaker().Generate(count);
    }
}