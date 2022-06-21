using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DS.Application.CQRS.MediaLibrary.Commands;
using DS.Application.CQRS.MediaLibrary.Queries;
using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.DataAccess.Context;
using DS.DataAccess.Seeding.Generators;
using DS.Domain;
using DS.Tests.Stubs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DS.Tests.Handlers;

[TestFixture]
public class MusicLibraryHandlersTests
{
    private MusicDbContext _context;
    private MusicUser _musicUser;
    private Song _song;
    private Playlist _playlist;
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MusicDbContext>()
            .UseInMemoryDatabase("ChurkaDb")
            .Options;
        _context = new MusicDbContext(options);

        _musicUser = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        _context.Add(_musicUser);

        _song = GenerateSongs(Helpers.Constants.SingleEntity, _musicUser).First();
        _playlist = GeneratePlaylists(_musicUser, Helpers.Constants.SingleEntity).First();
    }

    [TearDown]
    public void TearDown() => _context.Database.EnsureDeleted();
    
    [Test]
    public async Task AddPlaylist_PlaylistAdded()
    {
        var handler = new AddPlaylist.Handler(_context);
        
        var playlistToAdd = GeneratePlaylist();

        var command = new AddPlaylist.AddPlaylistCommand(_musicUser.Id, playlistToAdd.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.Contains(playlistToAdd, await GetPlaylists());
    }

    [Test]
    public async Task AddSong_SongAdded()
    {
        var handler = new AddSong.Handler(_context);

        var songToAdd = GenerateSongs(Helpers.Constants.SingleEntity, _musicUser).First();
        var musicUser = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        _context.Add(musicUser);
        
        var command = new AddSong.AddSongCommand(musicUser.Id, songToAdd.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.Contains(songToAdd, await GetMediaLibrarySongs(musicUser.Id));
    }

    [Test]
    public async Task CreateNewPlaylist_PlaylistCreated()
    {
        var storage = new SystemStorageStub();
        var handler = new CreateNewPlaylist.Handler(_context, storage);

        var songsIds = GenerateSongs(3, _musicUser).Select(s => s.Id).ToList();

        var playlistCreationDto = new PlaylistCreationInfoDto
        (
            _musicUser.Id,
            "Dummy",
            null,
            true,
            songsIds,
            FileStub.GetDummyFile()
        );
        var command = new CreateNewPlaylist.CreateNewPlaylistCommand(_musicUser.Id, playlistCreationDto);
        await handler.Handle(command, CancellationToken.None);

        var playlist = await GetPlaylistByAuthorId(_musicUser.Id);
        Assert.Contains(playlist, await GetPlaylists());
        Assert.True(Helpers.EntityExistsInDatabase(playlist, _context));
    }

    [Test]
    public async Task CreateNewSong_SongCreated()
    {
        var storage = new SystemStorageStub();
        var handler = new CreateNewSong.Handler(_context, storage);

        var genre = GenreGenerator.GenerateSongGenres(Helpers.Constants.SingleEntity).First();
        _context.Add(genre);
        
        var songCreationDto = new SongCreationInfoDto
        (
            "Dummy",
            genre.Id,
            _musicUser.Id,
            FileStub.GetDummyFile(),
            FileStub.GetDummyFile()
        );

        var command = new CreateNewSong.CreateNewSongCommand(_musicUser.Id, songCreationDto);
        await handler.Handle(command, CancellationToken.None);

        var song = await GetSongByAuthorId(_musicUser.Id);
        Assert.Contains(song, _context.Songs.ToList());
        Assert.True(Helpers.EntityExistsInDatabase(song, _context));
    }

    [Test]
    public async Task DeleteAuthoredPlaylist_PlaylistDeleted()
    {
        var newMusicUser = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        _context.Add(newMusicUser);

        await AddPlaylistToUser(_playlist, newMusicUser);

        var storage = new SystemStorageStub();
        var handler = new DeleteAuthoredPlaylist.Handler(_context, storage);

        var command = new DeleteAuthoredPlaylist.DeleteAuthoredPlaylistCommand(_musicUser.Id, _playlist.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.False(Helpers.EntityExistsInDatabase(_playlist, _context));
        foreach (var mediaLibrary in _context.MediaLibraries)
            CollectionAssert.DoesNotContain(mediaLibrary.Playlists, _playlist);
    }

    [Test]
    public async Task DeleteAuthoredSong_SongDeleted()
    {
        var newMusicUser = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        _context.Add(newMusicUser);

        await AddSongToUser(_song, newMusicUser);
        
        var storage = new SystemStorageStub();
        var handler = new DeleteAuthoredSong.Handler(_context, storage);
        
        var command = new DeleteAuthoredSong.DeleteAuthoredSongCommand(_musicUser.Id, _song.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.False(Helpers.EntityExistsInDatabase(_song, _context));
        foreach (var mediaLibrary in _context.MediaLibraries)
            CollectionAssert.DoesNotContain(mediaLibrary.Songs, _song);
    }

    [Test]
    public async Task DeletePlaylist_PlaylistDeleted()
    {
        var handler = new DeletePlaylist.Handler(_context);

        var command = new DeletePlaylist.DeletePlaylistCommand(_musicUser.Id, _playlist.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.True(Helpers.EntityExistsInDatabase(_playlist, _context));
        CollectionAssert.DoesNotContain(await GetPlaylists(), _playlist);
    }

    [Test]
    public async Task DeleteSong_SongDeleted()
    {
        var handler = new DeleteSong.Handler(_context);

        var command = new DeleteSong.DeleteSongCommand(_musicUser.Id, _song.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.True(Helpers.EntityExistsInDatabase(_song, _context));
        CollectionAssert.DoesNotContain(await GetMediaLibrarySongs(_musicUser.Id), _song);
    }

    [Test]
    public async Task GetAuthoredPlaylists_PlaylistsRetrieved()
    {
        var mapper = Helpers.GenerateMapper();

        var newAuthor = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        var playlistToAdd = GeneratePlaylists(newAuthor, Helpers.Constants.SingleEntity).First();

        await AddPlaylistToUser(playlistToAdd, _musicUser);
        
        var handler = new GetAuthoredPlaylists.Handler(_context, mapper);

        var query = new GetAuthoredPlaylists.GetAuthoredPlaylistsQuery(_musicUser.Id);
        var response = await handler.Handle(query, CancellationToken.None);

        var refDto = GenerateRefPlaylistDto(_playlist, _musicUser);
        var actualDto = response.AuthoredPlaylistsInfo.First();
        
        Assert.AreEqual(response.AuthoredPlaylistsInfo.Count, 1);
        Assert.AreEqual(refDto.Author, actualDto.Author);
        Assert.AreEqual(refDto.Name, actualDto.Name);
        CollectionAssert.AreEqual(refDto.Songs, actualDto.Songs);
    }

    [Test]
    public async Task GetAuthoredSongs_SongsRetrieved()
    {
        var mapper = Helpers.GenerateMapper();
        var refSongDto = GenerateRefSongDto(_song);
        
        var newAuthor = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        var songToAdd = GenerateSongs(Helpers.Constants.SingleEntity, newAuthor).First();

        await AddSongToUser(songToAdd, _musicUser);
        
        var handler = new GetAuthoredSongs.Handler(_context, mapper);

        var query = new GetAuthoredSongs.GetAuthoredSongsQuery(_musicUser.Id);
        var response = await handler.Handle(query, CancellationToken.None);
        
        Assert.AreEqual(1, response.AuthoredSongsInfo.Count);
        Assert.AreEqual(refSongDto, response.AuthoredSongsInfo.First());
    }

    [Test]
    public async Task GetPlaylists_PlaylistsRetrieved()
    {
        var mapper = Helpers.GenerateMapper();
        
        var newAuthor = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        var playlistToAdd = PlaylistGenerator.GeneratePlaylists
        (
            new List<Song> {_song},
            new List<MusicUser> { newAuthor },
            1
        ).First();
        _context.Add(playlistToAdd);

        await AddPlaylistToUser(playlistToAdd, _musicUser);
        
        var handler = new GetPlaylists.Handler(_context, mapper);

        var query = new GetPlaylists.GetPlaylistsQuery(_musicUser.Id);
        var response = await handler.Handle(query, CancellationToken.None);

        Assert.AreEqual(2, response.PlaylistsInfo.Count);
        
        var refDto1 = GenerateRefPlaylistDto(_playlist, _musicUser);
        var refDto2 = GenerateRefPlaylistDto(playlistToAdd, newAuthor);

        var actualDto1 = response.PlaylistsInfo.ElementAt(0);
        var actualDto2 = response.PlaylistsInfo.ElementAt(1);
        
        Assert.AreEqual(refDto1.Author, actualDto1.Author);
        Assert.AreEqual(refDto1.Name, actualDto1.Name);
        CollectionAssert.AreEqual(refDto1.Songs, actualDto1.Songs);
        
        Assert.AreEqual(refDto2.Author, actualDto2.Author);
        Assert.AreEqual(refDto2.Name, actualDto2.Name);
        CollectionAssert.AreEqual(refDto2.Songs, actualDto2.Songs);
    }

    [Test]
    public async Task GetSongs_SongsRetrieved()
    {
        var mapper = Helpers.GenerateMapper();
        
        var newAuthor = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        var songToAdd = GenerateSongs(Helpers.Constants.SingleEntity, newAuthor).First();
        await AddSongToUser(songToAdd, _musicUser);
        
        var refSongDto1 = GenerateRefSongDto(_song);
        var refSongDto2 = GenerateRefSongDto(songToAdd);

        var handler = new GetSongs.Handler(_context, mapper);

        var query = new GetSongs.GetSongsQuery(_musicUser.Id);
        var response = await handler.Handle(query, CancellationToken.None);
        
        Assert.AreEqual(2, response.SongsInfo.Count);
        Assert.AreEqual(refSongDto1, response.SongsInfo.ElementAt(0));
        Assert.AreEqual(refSongDto2, response.SongsInfo.ElementAt(1));
    }
    
    private async Task AddPlaylistToUser(Playlist playlist, MusicUser user)
    {
        var handler = new AddPlaylist.Handler(_context);

        var command = new AddPlaylist.AddPlaylistCommand(user.Id, playlist.Id);
        await handler.Handle(command, CancellationToken.None);
    }

    private async Task AddSongToUser(Song song, MusicUser user)
    {
        var handler = new AddSong.Handler(_context);

        var command = new AddSong.AddSongCommand(user.Id, song.Id);
        await handler.Handle(command, CancellationToken.None);
    }
    
    private async Task<List<Playlist>> GetPlaylists()
    {
        return (await _context.MusicUsers.FindAsync(_musicUser.Id))!
            .MediaLibrary
            .Playlists
            .ToList();
    }
    
    private async Task<Playlist> GetPlaylistByAuthorId(Guid authorId)
    {
        return (await _context.MusicUsers.FindAsync(_musicUser.Id))!
            .MediaLibrary
            .Playlists
            .First(p => p.Author.Id == authorId);
    }
    
    private async Task<Song> GetSongByAuthorId(Guid authorId)
    {
        return (await _context.MusicUsers.FindAsync(_musicUser.Id))!
            .MediaLibrary
            .Songs
            .First(p => p.Author.Id == authorId);
    }
    
    private async Task<List<Song>> GetMediaLibrarySongs(Guid id)
    {
        return (await _context.MusicUsers.FindAsync(id))!
            .MediaLibrary
            .Songs
            .ToList();
    }
    
    private Playlist GeneratePlaylist()
    {
        var playlistAuthor = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        var playlist = PlaylistGenerator.GeneratePlaylists
        (
            new List<Song> { _song }, 
            new List<MusicUser> { playlistAuthor }, 
            Helpers.Constants.SingleEntity
        ).First();
        
        _context.Add(playlist);
        
        return playlist;
    }
    
    private List<Playlist> GeneratePlaylists(MusicUser user, int count)
    {
        var playlists = PlaylistGenerator.GeneratePlaylists
        (
            new List<Song> { _song }, 
            new List<MusicUser> { user }, 
            count
        ).ToList();

        foreach (var playlist in playlists)
            _context.Add(playlist);
        
        return playlists;
    }
    
    private List<Song> GenerateSongs(int count, MusicUser author)
    {
        var songs =  SongGenerator.GenerateSongs
        (
            new List<MusicUser> { author },
            GenreGenerator.GenerateSongGenres(Helpers.Constants.SingleEntity),
            count
        ).ToList();

        foreach (var song in songs)
            _context.Add(song);

        return songs;
    }

    private PlaylistInfoDto GenerateRefPlaylistDto(Playlist playlist, MusicUser author)
    {
        var songInfoDto = new SongInfoDto
        (
            _song.Name,
            _song.Genre.Name,
            _song.Author.Name,
            FileStub.GetResultFileDummy(),
            FileStub.GetResultFileDummy()
        );

        var musicUserInfoDto = new MusicUserInfoDto(author.Id, author.Name, FileStub.GetResultFileDummy());
        
        return new PlaylistInfoDto
            (
                playlist.Name, 
                new List<SongInfoDto> { songInfoDto },
                FileStub.GetResultFileDummy(),
                musicUserInfoDto
            );
    }

    private SongInfoDto GenerateRefSongDto(Song song)
    {
        return new SongInfoDto
        (
            song.Name,
            song.Genre.Name,
            song.Author.Name,
            FileStub.GetResultFileDummy(),
            FileStub.GetResultFileDummy()
        );
    }
}