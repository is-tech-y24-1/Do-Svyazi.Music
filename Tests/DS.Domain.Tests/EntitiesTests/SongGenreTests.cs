using System;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

public class SongGenreTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void CreateSongGenreEntity_NoExceptions()
    {
        Assert.DoesNotThrow(() =>
        {
            var songGenre = new SongGenre("Blues");
        });
    }

    [Test]
    public void CreateSongGenreEntity_ThrowException()
    {
        Assert.Catch<Exception>(() =>
        {
            var songGenre1 = new SongGenre("");
            var songGenre2 = new SongGenre(" ");
        });
    }
}