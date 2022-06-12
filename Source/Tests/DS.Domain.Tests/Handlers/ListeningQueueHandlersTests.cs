using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DS.Application.CQRS.ListeningQueue.Commands;
using DS.Application.CQRS.ListeningQueue.Queries;
using DS.Application.CQRS.Mapping;
using DS.Application.DTO.ListeningQueue;
using DS.Application.DTO.Song;
using DS.DataAccess.Context;
using DS.DataAccess.Seeding.Generators;
using DS.Domain;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DS.Tests.Handlers;

[TestFixture]
public class ListeningQueueHandlersTests
{
    private MusicDbContext _context;
    private MusicUser _musicUser;
    private Song _firstSong;
    private Song _secondSong;

    [SetUp]
    public void SetUp()
    {
        DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>()
            .UseInMemoryDatabase("MockDb")
            .Options;
        _context = new MusicDbContext(options);
        
        _musicUser = MusicUserGenerator.GenerateMusicUsers(1).First();
        _context.Add(_musicUser);

        var songs = GenerateSongs(2);
        
        foreach (var song in songs)
            _context.Add(song);
        
        _firstSong = songs[0];
        _secondSong = songs[1];
    }

    [TearDown]
    public void TearDown() => _context.Database.EnsureDeleted();
    
    [Test]
    public async Task AddLastToQueue_LastSongAdded()
    {
        var handler = new AddLastToQueue.Handler(_context);
        
        var command = new AddLastToQueue.AddLastToQueueCommand(_musicUser.Id, _firstSong.Id);
        await handler.Handle(command, CancellationToken.None);

        Assert.Contains(_firstSong, await GetQueueSongs());
    }

    [Test]
    public async Task AddNextToQueue_NextSongAdded()
    {
        var handler = new AddNextToQueue.Handler(_context);
        
        var command = new AddNextToQueue.AddNextToQueueCommand(_musicUser.Id, _firstSong.Id);
        await handler.Handle(command, CancellationToken.None);

       Assert.Contains(_firstSong, await GetQueueSongs());
    }

    [Test]
    public async Task ChangeQueueSongPosition_PositionChanged()
    {
        var handler = new ChangeQueueSongPosition.Handler(_context);

        await PopulateQueueWithSongs();

        var changeSongsPositionCommand = 
            new ChangeQueueSongPosition.ChangeQueueSongPositionCommand
            (
                _musicUser.Id,
                _firstSong.Id, 
                1
            );
        await handler.Handle(changeSongsPositionCommand, CancellationToken.None);

        Assert.Contains(_firstSong, await GetQueueSongs());
        Assert.Contains(_secondSong, await GetQueueSongs());
    }

    [Test]
    public async Task ClearQueue_QueueCleared()
    {
        var handler = new ClearQueue.Handler(_context);

        await PopulateQueueWithSongs();

        var clearQueueCommand = new ClearQueue.ClearQueueCommand(_musicUser.Id);
        await handler.Handle(clearQueueCommand, CancellationToken.None);
        
        Assert.IsEmpty(await GetQueueSongs());
        Assert.True(SongExistsInDatabase(_firstSong));
        Assert.True(SongExistsInDatabase(_secondSong));
    }

    [Test]
    public async Task DeleteSongFromQueue_SongDeleted()
    {
        var handler = new DeleteFromQueue.Handler(_context);
        
        await PopulateQueueWithSongs();
        
        var command = new DeleteFromQueue.DeleteFromQueueCommand(_musicUser.Id, _firstSong.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.True(SongExistsInDatabase(_firstSong));
        Assert.False((await GetQueueSongs()).Contains(_firstSong));
        Assert.Contains(_secondSong, await GetQueueSongs());
    }

    [Test]
    public async Task DeleteSongFromQueue_SongDeleted_DeletingFromTheMiddle()
    {
        var handler = new DeleteFromQueue.Handler(_context);

        var songs = GenerateSongs(5);
        await PopulateQueueWithSongs(songs);
        
        var command = new DeleteFromQueue.DeleteFromQueueCommand(_musicUser.Id, songs[2].Id);
        await handler.Handle(command, CancellationToken.None);

        var refSongsOrder = new List<Song> { songs[0], songs[1], songs[3], songs[4] };
        var actualSongs = await GetQueueSongs();
        
        for (int i = 0; i < actualSongs.Count; i++)
        {
            if (!refSongsOrder[i].Equals(actualSongs[i]))
                Assert.Fail();
        }
        
        Assert.True(SongExistsInDatabase(songs[2]));
        CollectionAssert.DoesNotContain(await GetQueueSongs(),songs[2]);
    }

    [Test]
    public async Task GetQueueInfo_InformationRetrieved()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<DomainToResponse>();
        });
        var mapper = mapperConfig.CreateMapper();
        
        await PopulateQueueWithSongs();

        var handler = new GetQueueInfo.Handler(_context, mapper);
        
        var command = new GetQueueInfo.GetInfoQuery(_musicUser.Id);
        var response = await handler.Handle(command, CancellationToken.None);

        var refDto = GenerateRefDto();

        Assert.AreEqual(refDto.OwnerId, response.QueueInfo.OwnerId);
        CollectionAssert.AreEqual(refDto.Songs, response.QueueInfo.Songs);
    }
    
    private bool SongExistsInDatabase(Song song) => _context.Set<Song>().Local.Any(e => e.Equals(song));

    private async Task<List<Song>> GetQueueSongs()
    {
        return (await _context.MusicUsers.FindAsync(_musicUser.Id))!
            .ListeningQueue
            .Songs
            .ToList();
    }
    
    private async Task PopulateQueueWithSongs()
    {
        var queueHandler = new AddLastToQueue.Handler(_context);
        
        var addFirstSongCommand = new AddLastToQueue.AddLastToQueueCommand(_musicUser.Id, _firstSong.Id);
        var addSecondSongCommand =  new AddLastToQueue.AddLastToQueueCommand(_musicUser.Id, _secondSong.Id);

        await queueHandler.Handle(addFirstSongCommand, CancellationToken.None);
        await queueHandler.Handle(addSecondSongCommand, CancellationToken.None);
    }

    private async Task PopulateQueueWithSongs(List<Song> songs)
    {
        var handler = new AddLastToQueue.Handler(_context);

        foreach (var song in songs)
            _context.Add(song);

        foreach (var song in songs)
        {
            var command = new AddLastToQueue.AddLastToQueueCommand(_musicUser.Id, song.Id);
            await handler.Handle(command, CancellationToken.None);
        }
    }

    private List<Song> GenerateSongs(int count)
    {
        return SongGenerator.GenerateSongs
        (
            new List<MusicUser> {_musicUser},
            GenreGenerator.GenerateSongGenres(1),
            count
        ).ToList();
    }

    private ListeningQueueInfoDto GenerateRefDto()
    {
        return new ListeningQueueInfoDto(_musicUser.Id, new List<SongInfoDto>()
        {
            new SongInfoDto(_firstSong.Name, _firstSong.Genre.Name, _firstSong.Author.Name, _firstSong.ContentUri,
                _firstSong.CoverUri),
            new SongInfoDto(_secondSong.Name, _secondSong.Genre.Name, _secondSong.Author.Name, _secondSong.ContentUri,
                _secondSong.CoverUri)
        });
    }
}