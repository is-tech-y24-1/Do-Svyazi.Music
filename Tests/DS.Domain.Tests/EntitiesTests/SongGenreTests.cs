using System;
using DS.Common.Exceptions;
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

    [TestCase("")]
    [TestCase(" ")]
    public void CreateSongGenreEntityWithInvalidName_ThrowException(string invalidName)
    {
        Assert.Catch<DoSvyaziMusicException>(() =>
        {
            var songGenre = new SongGenre(invalidName);
        });
    }
}