using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DS.Application.CQRS.ListeningQueue.Commands;
using DS.Application.CQRS.ListeningQueue.Queries;
using DS.Application.DTO.ListeningQueue;
using DS.Application.DTO.Song;
using DS.DataAccess.Context;
using DS.DataAccess.Seeding.Generators;
using DS.Domain;
using DS.Tests.Stubs;
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
        var options = new DbContextOptionsBuilder<MusicDbContext>()
            .UseInMemoryDatabase("ChurkaDb")
            .Options;
        _context = new MusicDbContext(options);
        
        _musicUser = MusicUserGenerator.GenerateMusicUsers(Helpers.Constants.SingleEntity).First();
        _context.Add(_musicUser);

        var songs = GenerateSongs(2);
        
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
                Helpers.Constants.SingleEntity
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
        Assert.True(Helpers.EntityExistsInDatabase(_firstSong, _context));
        Assert.True(Helpers.EntityExistsInDatabase(_secondSong, _context));
    }

    [Test]
    public async Task DeleteSongFromQueue_SongDeleted()
    {
        var handler = new DeleteFromQueue.Handler(_context);
        
        await PopulateQueueWithSongs();
        
        var command = new DeleteFromQueue.DeleteFromQueueCommand(_musicUser.Id, _firstSong.Id);
        await handler.Handle(command, CancellationToken.None);
        
        Assert.True(Helpers.EntityExistsInDatabase(_firstSong, _context));
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
        
        Assert.True(Helpers.EntityExistsInDatabase(songs[2], _context));
        CollectionAssert.AreEqual(refSongsOrder, actualSongs);
        CollectionAssert.DoesNotContain(await GetQueueSongs(),songs[2]);
    }

    [Test]
    public async Task GetQueueInfo_InformationRetrieved()
    {
        var mapper = Helpers.GenerateMapper();
        
        await PopulateQueueWithSongs();

        var handler = new GetQueueInfo.Handler(_context, mapper);
        
        var query = new GetQueueInfo.GetInfoQuery(_musicUser.Id);
        var response = await handler.Handle(query, CancellationToken.None);

        var refDto = GenerateRefDto();

        Assert.AreEqual(refDto.OwnerId, response.QueueInfo.OwnerId);
        CollectionAssert.AreEqual(refDto.Songs, response.QueueInfo.Songs);
    }

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
        {
            var command = new AddLastToQueue.AddLastToQueueCommand(_musicUser.Id, song.Id);
            await handler.Handle(command, CancellationToken.None);
        }
    }

    private List<Song> GenerateSongs(int count)
    {
        var songs = SongGenerator.GenerateSongs
        (
            new List<MusicUser> {_musicUser},
            GenreGenerator.GenerateSongGenres(Helpers.Constants.SingleEntity),
            count
        ).ToList();

        foreach (var song in songs)
            _context.Add(song);

        return songs;
    }

    private ListeningQueueInfoDto GenerateRefDto()
    {
        return new ListeningQueueInfoDto(_musicUser.Id, new List<SongInfoDto>()
        {
            new (_firstSong.Name, _firstSong.Genre.Name, _firstSong.Author.Name),
            new (_secondSong.Name, _secondSong.Genre.Name, _secondSong.Author.Name)
        });
    }
}