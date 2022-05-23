using System;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests;

public class EntityTests
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
            var songGenre = new SongGenre("");
        });
    }
}