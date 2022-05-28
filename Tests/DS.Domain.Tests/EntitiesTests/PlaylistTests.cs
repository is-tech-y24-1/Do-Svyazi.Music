using System;
using System.Linq;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

[TestFixture]
public class PlaylistTests
{
    private Playlist _playlist;
    private MusicUser _author;
    private PlaylistSongs _songs;
    private Song _song1;
    private Song _song2;
    
    [SetUp]
    public void Setup()
    {
        _author = new MusicUser();
        _songs = new PlaylistSongs();
        _playlist = new Playlist("Саундтреки из аниме", _author, _songs, true);
        _song1 = new Song("Deep Purple - Highway star", new SongGenre("Rock"), _author, "Test");
        _song2 = new Song("Rodriguez - I wonder", new SongGenre("Rock"), _author, "Test");
    }

    [Test]
    public void AddSongToPlaylist_SongAdded()
    {
        _playlist.AddSong(_song1);
        Assert.Contains(_song1, _playlist.Songs.ToList());
    }


    [Test]
    public void ChangePlaylistName_NameChanged()
    {
        const string newName = "Плейлист для написания кода в ванной";
        _playlist.Name = newName;
        Assert.AreEqual(newName, _playlist.Name);
    }

    [Test]
    public void DeleteSongFromPlaylist_SongDeleted()
    {
        _playlist.AddSong(_song1);
        _playlist.DeleteSong(_song1);
        Assert.False(_playlist.Songs.ToList().Contains(_song1));
    }

    [Test]
    public void ChangeSongPositionInPlaylist_PositionsChanges()
    {
        _playlist.AddSong(_song1);
        _playlist.AddSong(_song2);
        _playlist.ChangeSongPosition(_song2, 0);

        Assert.True(_playlist.Songs.ToList()[0].Equals(_song2));
        Assert.True(_playlist.Songs.ToList()[1].Equals(_song1));
    }
}