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
        var playlistSongs = new PlaylistSongs(_songsToTest[0]);
        playlistSongs.Insert(0, _songsToTest[2]);
        playlistSongs.Insert(0, _songsToTest[1]);
        
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
}