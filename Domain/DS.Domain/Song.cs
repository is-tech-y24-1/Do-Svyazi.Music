using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class Song
{
    private List<MusicUser> _featuring = new ();
    
    #pragma warning disable CS8618
    public Song() { }
    #pragma warning restore CS8618

    public Song(string name, SongGenre genre, MusicUser author, string songContentUri)
    {
        Name = name.ThrowIfNull();
        Genre = genre.ThrowIfNull();
        Author = author.ThrowIfNull();
        SongContentUri = songContentUri.ThrowIfNull();

        if (string.IsNullOrWhiteSpace(name))
            throw new DoSvyaziMusicException("Name cannot be empty");
        if (string.IsNullOrWhiteSpace(songContentUri))
            throw new DoSvyaziMusicException("Content uri cannot be null");
        
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; private init; }
    public string Name { get; set; }
    public SongGenre Genre { get; set; }
    public MusicUser Author { get; private init; }
    public IReadOnlyCollection<MusicUser> Featuring => _featuring;
    public string? CoverUri { get; set; }
    public bool SharedForCommunity { get; set; }
    public string SongContentUri { get; private init; }

    public void AddFeaturing(MusicUser featuringUser)
    {
        featuringUser.ThrowIfNull();
        _featuring.Add(featuringUser);
    }

    public void DeleteFeaturingUser(MusicUser featuringUser)
    {
        _featuring.ThrowIfNull();
        
        var userToDelete = _featuring.Find(user => user.Id == featuringUser.Id);
        if (userToDelete is null)
            throw new DoSvyaziMusicException("There is no such user to delete");

        _featuring.Remove(userToDelete);
    }
}