using System;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

[TestFixture]
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
        Assert.Catch<ArgumentNullException>(() =>
        {
            var songGenre = new SongGenre(invalidName);
        });
    }
}