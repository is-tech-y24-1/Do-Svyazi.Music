using DS.Common.Exceptions;
using DS.Domain;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

public class SongTests
{
    private MusicUser _featuringUser;
    private MusicUser _author;
    private Song _song;
    
    [SetUp]
    public void Setup()
    {
        _featuringUser = new MusicUser();
        _author = new MusicUser();
        _song = new Song("Test", new SongGenre("Test"), _author, "Content");
    }

    [Test]
    public void AddFeaturingUser_UserIsNotNull_Success()
    {
        // when
        _song.AddFeaturing(_featuringUser);
        
        // then
        Assert.Contains(_featuringUser, _song.Featuring);
    }

    [Test]
    public void DeleteFeaturingUser_UserIsFeaturing_UserDeleted()
    {
        // given
        _song.AddFeaturing(_featuringUser);
        
        // when
        _song.DeleteFeaturingUser(_featuringUser);
        
        // then
        Assert.False(_song.Featuring.Contains(_featuringUser));
    }

    [Test]
    public void DeleteFeaturingUser_UserIsNotFeaturing_ThrowError()
    {
        Assert.Catch(() =>
        {
            _song.DeleteFeaturingUser(_featuringUser);
        });
    }
}