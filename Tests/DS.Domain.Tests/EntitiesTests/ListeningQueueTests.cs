using System;
using System.Collections.Generic;
using System.Linq;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

[TestFixture]
public class ListeningQueueTests
{
    private ListeningQueue _queue;
    private MusicUser _owner;
    private List<Song> _testSongs;

    [SetUp]
    public void SetUp()
    {
        _owner = new MusicUser(Guid.NewGuid(), "Test", "Test");
        _queue = new ListeningQueue(_owner.Id);
        var author = new MusicUser(Guid.NewGuid(), "Test", "Test");

        _testSongs = new List<Song>()
        {
            new Song("dummy0", new SongGenre("dummy"), author, "dummy"),
            new Song("dummy1", new SongGenre("dummy"), author, "dummy"),
            new Song("dummy2", new SongGenre("dummy"), author, "dummy"),
            new Song("dummy3", new SongGenre("dummy"), author, "dummy"),
            new Song("dummy4", new SongGenre("dummy"), author, "dummy"),
            new Song("dummy5", new SongGenre("dummy"), author, "dummy"),
            new Song("dummy6", new SongGenre("dummy"), author, "dummy"),
        };
        foreach (var s in _testSongs)
            _queue.AddLastToPlay(s);
    }

    [Test]
    public void SwitchSongWithLastSong_PositionChanged()
    {
        _queue.ChangeSongPosition(_testSongs[0], _testSongs[6]);
        Assert.AreEqual(_testSongs[0].Id, _queue.Songs.ToList()[6].Id);
        Assert.AreNotEqual(_testSongs[0].Id, _queue.Songs.ToList()[0].Id);
    }

    [Test]
    public void SwitchSongWithSongInTheMiddle_PositionChanged()
    {
        _queue.ChangeSongPosition(_testSongs[0], _testSongs[3]);
        Assert.AreEqual(_testSongs[0].Id, _queue.Songs.ToList()[3].Id);
        Assert.AreNotEqual(_testSongs[0].Id, _queue.Songs.ToList()[0].Id);
    }
}