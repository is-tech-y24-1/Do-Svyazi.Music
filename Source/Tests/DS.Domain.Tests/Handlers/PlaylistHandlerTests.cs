using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DS.Application.CQRS.Playlist.Commands;
using DS.Application.CQRS.Playlist.Queries;
using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.DataAccess.Context;
using DS.DataAccess.Seeding.Generators;
using DS.Domain;
using DS.Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DS.Tests.Handlers;

[TestFixture]
public class PlaylistHandlerTests
{
    private MusicDbContext _context;
    private Playlist _playlist;
    private MusicUser _musicUser;
    private Song _initialSong;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MusicDbContext>()
            .UseInMemoryDatabase("ChurkaDb")
            .Options;
        _context = new MusicDbContext(options);
        
        _musicUser = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        _context.Add(_musicUser);
        
        _initialSong = GenerateSongs(Helpers.Constants.SingleEntity, _musicUser).First();
        _playlist = GeneratePlaylist(_musicUser);
    }

    [TearDown]
    public void TearDown() => _context.Database.EnsureDeleted();
    
    [Test]
    public async Task AddSongToPlaylist_SongAdded()
    {
        var songToAdd = GenerateSongs(Helpers.Constants.SingleEntity, _musicUser).First();

        await AddSongToPlaylist(songToAdd, _playlist);
            
        CollectionAssert.Contains(GetPlaylistsSongs(_playlist.Id), songToAdd);
    }

    [Test]
    public async Task ChangePlaylistSongPosition_PositionChanged()
    {
        var secondSong = GenerateSongs(Helpers.Constants.SingleEntity, _musicUser).First();
        
        await AddSongToPlaylist(secondSong, _playlist);

        var handler = new ChangePlaylistSongPosition.Handler(_context);
        var command = new ChangePlaylistSongPosition.ChangePositionCommand
        (
            _musicUser.Id,
            _playlist.Id,
            _initialSong.Id, 
            1
        );
        await handler.Handle(command, CancellationToken.None);
        
        var refSongPosition = new List<Song> {secondSong, _initialSong};
        var actualSongsPosition = GetPlaylistsSongs(_playlist.Id);
        
        CollectionAssert.AreEqual(refSongPosition, actualSongsPosition);
    }

    [Test]
    public async Task DeleteSongFromPlaylist_SongDeleted()
    {
        var song = GenerateSongs(Helpers.Constants.SingleEntity, _musicUser).First();
        
        await AddSongToPlaylist(song, _playlist);

        var handler = new DeleteSongFromPlaylist.Handler(_context);
        var command = new DeleteSongFromPlaylist.DeletePlaylistSongCommand
        (
            _musicUser.Id,
            _playlist.Id,
            song.Id
        );
        await handler.Handle(command, CancellationToken.None);
        
        Assert.True(Helpers.EntityExistsInDatabase(song, _context));
        CollectionAssert.DoesNotContain(GetPlaylistsSongs(_playlist.Id), song);
    }

    [Test]
    public async Task GetPlaylistInfo_InfoRetrieved()
    {
        var mapper = Helpers.GenerateMapper();

        var handler = new GetPlaylistInfo.Handler(_context, mapper);
        var query = new GetPlaylistInfo.GetInfoQuery(_musicUser.Id, _playlist.Id);
        
        var response = await handler.Handle(query, CancellationToken.None);
        
        var refPlaylistInfoDto = GenerateRefPlaylistDto();
        var actualPlaylistInfoDto = response.PlaylistInfo;
        
        Assert.AreEqual(refPlaylistInfoDto.Author, actualPlaylistInfoDto.Author);
        Assert.AreEqual(refPlaylistInfoDto.Name, actualPlaylistInfoDto.Name);
        CollectionAssert.AreEqual(refPlaylistInfoDto.Songs, actualPlaylistInfoDto.Songs);
        
    }
    private Playlist GeneratePlaylist(MusicUser author)
    {
        var playlist = PlaylistGenerator.GeneratePlaylists
        (
            new List<Song> { _initialSong }, 
            new List<MusicUser> { author }, 
            1
        ).First();
        
        _context.Add(playlist);
        
        return playlist;
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

    private PlaylistInfoDto GenerateRefPlaylistDto()
    {
        
        var songInfoDto = new SongInfoDto
        (
            _initialSong.Name,
            _initialSong.Genre.Name,
            _initialSong.Author.Name
        );

        var musicUserInfoDto = new MusicUserInfoDto(_musicUser.Id, _musicUser.Name);
        
        return new PlaylistInfoDto
        (
            _playlist.Name, 
            new List<SongInfoDto> { songInfoDto },
            musicUserInfoDto
        );
    }
    
    private IEnumerable<Song> GetPlaylistsSongs(Guid playlistId)
    {
        return _context.Playlists
            .First(pl => pl.Id == playlistId)
            .Songs
            .ToList();
    }

    private async Task AddSongToPlaylist(Song song, Playlist playlist)
    {
        var handler = new AddSongToPlaylist.Handler(_context);

        var command = new AddSongToPlaylist.AddPlaylistSongCommand(_musicUser.Id, playlist.Id, song.Id);
        await handler.Handle(command, CancellationToken.None);
    }
}