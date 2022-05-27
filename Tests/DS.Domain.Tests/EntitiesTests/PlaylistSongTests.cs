using System.Collections.Generic;
using System.Linq;
using DS.Common.Exceptions;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

[TestFixture]
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
    public void AddSongsToPlaylistSong_SongsAdded()
    {
        var playlistSongs = new PlaylistSongs(_songsToTest[0]);
        playlistSongs.Add(_songsToTest[1]);
        playlistSongs.Add(_songsToTest[2]);

        CollectionAssert.AreEqual(_songsToTest, playlistSongs);
        
        foreach (var t in _songsToTest)
        {
            Assert.IsTrue(playlistSongs.Contains(t));
            Assert.IsTrue(playlistSongs.Contains(t));
        }
    }

    [Test]
    public void InsertSongAfterExisting_SongInserted()
    {
        var playlistSongs = new PlaylistSongs(_songsToTest[2]);
        playlistSongs.Insert(0, _songsToTest[1]);
        playlistSongs.Insert(0, _songsToTest[0]);
        
        CollectionAssert.AreEqual(_songsToTest, playlistSongs);
    }

    [Test]
    public void InsertSongAfterUnknownSong_ThrowsException()
    {
        var playlistSongs = new PlaylistSongs(_songsToTest.SkipLast(1).ToList());
        var unknownSong = new Song("_", new SongGenre("TestGenre"), new MusicUser(), "_");

        Assert.Throws<DoSvyaziMusicException>(() =>
        {
            playlistSongs.Insert(2, unknownSong);
        });
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void RemoveSong_SongRemoved(int positionToRemove)
    {
        var playlistSongs = new PlaylistSongs(_songsToTest);

        playlistSongs.Remove(_songsToTest[positionToRemove]);
        _songsToTest.RemoveAt(positionToRemove);

        CollectionAssert.AreEqual(_songsToTest, playlistSongs);
    }
    
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void RemoveAtSong_SongRemoved(int positionToRemove)
    {
        var playlistSongs = new PlaylistSongs(_songsToTest);

        playlistSongs.RemoveAt(positionToRemove);
        _songsToTest.RemoveAt(positionToRemove);

        CollectionAssert.AreEqual(_songsToTest, playlistSongs);
    }

    [Test]
    public void ClearSongsAndRefill_ClearedAndRefilled()
    {
        var playlistSongs = new PlaylistSongs(_songsToTest);
        playlistSongs.Clear();
        CollectionAssert.AreEqual(new List<Song>(), playlistSongs);
        
        playlistSongs.Add(_songsToTest[0]);
        playlistSongs.Add(_songsToTest[2]);
        playlistSongs.Insert(1, _songsToTest[1]);
        CollectionAssert.AreEqual(_songsToTest, playlistSongs);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void CopyToArrayFromIndex_CopiedSuccessful(int index)
    {
        var array = new Song[index + _songsToTest.Count];
        var playlistSongs = new PlaylistSongs(_songsToTest);
        
        playlistSongs.CopyTo(array, index);
        CollectionAssert.AreEqual(array.Skip(index).ToArray(), playlistSongs);
    }

    [TestCase(-1)]
    [TestCase(-10)]
    [TestCase(3)]
    [TestCase(5)]
    public void TryGetAndSetOutOfRangeElement_ThrowsException(int index)
    {
        var playlistSongs = new PlaylistSongs(_songsToTest);
        Assert.Throws<DoSvyaziMusicException>(() =>
        {
            playlistSongs[index].Name = "123";
        });
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void GetAndSetSongAtIndex_AllDone(int index)
    {
        var playlistSongs = new PlaylistSongs(_songsToTest);
        var element = playlistSongs[index];
        Assert.AreEqual(_songsToTest[index], element);
        Assert.AreEqual(index, playlistSongs.IndexOf(element));
        
        var newSong = new Song("aaa", new SongGenre("TestGenre"), new MusicUser(), "_");
        playlistSongs[index] = newSong;
        Assert.AreEqual(newSong, playlistSongs[index]);
        Assert.AreEqual(index, playlistSongs.IndexOf(newSong));
        
        _songsToTest[index] = newSong;
        CollectionAssert.AreEqual(_songsToTest, playlistSongs);
    }
}