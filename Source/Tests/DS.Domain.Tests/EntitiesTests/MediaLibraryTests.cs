using System;
using System.Linq;
using DS.Common.Exceptions;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

[TestFixture]
public class MediaLibraryTests
{
    private MusicUser? _owner;
    private MediaLibrary? _mediaLibrary;

    private Song? _song;
    
    [SetUp]
    public void Setup()
    {
        _owner = new MusicUser(Guid.NewGuid(), "test");
        _mediaLibrary = new MediaLibrary(_owner.Id);

        _song = new Song("Layla", new SongGenre("Blues"), _owner, "Test");
    }

    [Test]
    public void AddSongToLibrary_SongAdded()
    {
        _mediaLibrary!.AddSong(_song);
        Assert.Contains(_song, _mediaLibrary.Songs.ToList());
    }

    [Test]
    public void RemoveSongFromLibrary_SongRemoved()
    {
        _mediaLibrary!.AddSong(_song!);
        _mediaLibrary.DeleteSong(_song!);
        Assert.False(_mediaLibrary.Songs.Contains(_song));
    }

    [Test]
    public void AddAlreadyAddedSong_ThrowException()
    {
        _mediaLibrary!.AddSong(_song!);
        Assert.Catch<DoSvyaziMusicException>(() =>
        {
            _mediaLibrary.AddSong(_song!);
        });
    }
}