using System.Xml.Linq;
using DS.Common.Exceptions;
using DS.Domain;
using DS.Common.Extensions;
namespace DS.Domain;

public class Song
{
    #pragma warning disable CS8618
    public Song() { }
    #pragma warning restore CS8618

    public Song(string name, SongGenre genre, MusicUser author, string songContentUri)
    {
        Name = name;
        Genre = genre;
        Author = author;
        SongContentUri = songContentUri;
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; private init; }
    public string Name { get; set; }
    public SongGenre Genre { get; set; }
    public MusicUser Author { get; private init; }
    public List<MusicUser>? Featuring { get; private set; }
    public string? CoverUri { get; set; }
    public bool SharedForCommunity { get; set; }
    public string SongContentUri { get; private init; }

    public void AddFeaturing(MusicUser featuringUser)
    {
        Featuring ??= new List<MusicUser>();
        featuringUser.ThrowIfNullOrEmpty("User cannot be empty.");
        
        Featuring.Add(featuringUser);
    }

    public void DeleteFeaturingUser(MusicUser featuringUser)
    {
        Featuring.ThrowIfNullOrEmpty("There are no featuring users to delete");
        
        var userToDelete = Featuring?.Find(user => user.Id == featuringUser.Id);
        userToDelete.ThrowIfNullOrEmpty("There is no such featuring user.");

        Featuring?.Remove(userToDelete);
    }
}