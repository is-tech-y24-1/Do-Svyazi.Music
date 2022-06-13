using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DS.Application.CQRS.MusicUser.Commands;
using DS.Application.CQRS.MusicUser.Queries;
using DS.Application.DTO.MusicUser;
using DS.DataAccess.Context;
using DS.DataAccess.Seeding.Generators;
using DS.Domain;
using DS.Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DS.Tests.Handlers;

[TestFixture]
public class MusicUserHandlersTests
{
    private MusicDbContext _context;
    
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MusicDbContext>()
            .UseInMemoryDatabase("ChurkaDb")
            .Options;
        _context = new MusicDbContext(options);
    }

    [TearDown]
    public void TearDown() => _context.Database.EnsureDeleted();
    
    [Test]
    public async Task AddMusicUser_UserAdded()
    {
         var musicUser = MusicUserGenerator.GenerateMusicUsers(1).First();
        
         await  AddUserToDataBase(musicUser);
        
         Assert.True(Helpers.EntityExistsInDatabase(musicUser, _context));
    }

    [Test]
    public async Task GetMusicUserInfo_InfoRetrieved()
    {
        var musicUser = MusicUserGenerator.GenerateMusicUsers(1).First();
        
        await AddUserToDataBase(musicUser);
        
        var mapper = Helpers.GenerateMapper();
        
        var handler = new GetUserInfo.Handler(_context, mapper);
        var query = new GetUserInfo.GetInfoQuery(musicUser.Id);

        var refDto = new MusicUserInfoDto(musicUser.Id, musicUser.Name);
        var response = await handler.Handle(query, CancellationToken.None);
        
        Assert.AreEqual(refDto, response.UserInfo);
    }

    private async Task AddUserToDataBase(MusicUser user)
    {
        var userDto = new MusicUserCreationInfoDto(user.Id, user.Name);
        
        var storage = new SystemStorageStub();
        var handler = new AddMusicUser.Handler(_context, storage);

        var command = new AddMusicUser.AddUserCommand(userDto);
        await handler.Handle(command, CancellationToken.None);
    }
}