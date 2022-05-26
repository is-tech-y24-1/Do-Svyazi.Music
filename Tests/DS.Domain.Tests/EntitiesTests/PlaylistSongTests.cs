using System.Collections.Generic;
using System.Linq;
using DS.Common.Exceptions;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

public class PlaylistSongTests
{
    private static List<Song> _songsToTest = new ();
    
    [SetUp]
    public void Setup()
    {
        _songsToTest = new List<Song>
        {
            new Song("test1", new SongGenre("TestGenre") , new MusicUser(), "_"),
            new Song("test2", new SongGenre("TestGenre") , new MusicUser(), "_"),
            new Song("test3", new SongGenre("TestGenre") , new MusicUser(), "_")
        };
    }

    [Test]
    public void AddSongsLastToPlaylistSong_SongsAdded()
    {
        var start = new PlaylistSong(_songsToTest[0]);
        start.AddLast(_songsToTest[1]);
        start.AddLast(_songsToTest[2]);

        CollectionAssert.AreEqual(_songsToTest, start.ToList());
        
        foreach (var t in _songsToTest)
        {
            Assert.AreEqual(t, start.FindSong(t));
            Assert.IsTrue(start.Contains(t));
        }
    }
    
    [Test]
    public void AddSongsFirstToPlaylistSong_SongsAdded()
    {
        var start = new PlaylistSong(_songsToTest[2]);
        start.AddFirst(_songsToTest[1]);
        start.AddFirst(_songsToTest[0]);

        CollectionAssert.AreEqual(_songsToTest, start.ToList());
        
        foreach (var t in _songsToTest)
        {
            Assert.AreEqual(t, start.FindSong(t));
            Assert.IsTrue(start.Contains(t));
        }
    }

    [Test]
    public void InsertSongAfterExisting_SongInserted()
    {
        var start = new PlaylistSong(_songsToTest[0]);
        start.InsertAfter(_songsToTest[2], _songsToTest[0]);
        start.InsertAfter(_songsToTest[1], _songsToTest[0]);
        
        CollectionAssert.AreEqual(_songsToTest, start.ToList());
    }

    [Test]
    public void InsertSongAfterUnknownSong_ThrowsException()
    {
        var start = new PlaylistSong(_songsToTest[0]);
        start.AddLast(_songsToTest[1]);
        var unknownSong = new Song("_", new SongGenre("TestGenre"), new MusicUser(), "_");

        Assert.Throws<EntityNotFoundException>(() =>
        {
            start.InsertAfter(_songsToTest[2], unknownSong);
        });
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void RemoveSong_SongRemoved(int positionToRemove)
    {
        var start = new PlaylistSong(_songsToTest[0]);
        start.AddLast(_songsToTest[1]);
        start.AddLast(_songsToTest[2]);

        start.Remove(_songsToTest[positionToRemove]);
        _songsToTest.RemoveAt(positionToRemove);
        
        CollectionAssert.AreEqual(_songsToTest, start.ToList());
    }

    [Test]
    public void RemoveOnlySong_ThrowsException()
    {
        var start = new PlaylistSong(_songsToTest[0]);
        start.AddLast(_songsToTest[1]);
        
        start.Remove(_songsToTest[0]);

        Assert.Throws<DoSvyaziMusicException>(() =>
        {
            start.Remove(_songsToTest[1]);
        });
    }
}