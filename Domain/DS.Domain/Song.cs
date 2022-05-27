using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class Song : IEquatable<Song>
{
    private List<MusicUser> _featuring = new ();
    
    #pragma warning disable CS8618
    protected Song() { }
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

    public void AddFeaturingUser(MusicUser featuringUser)
    {
        featuringUser.ThrowIfNull();
        if (_featuring.Contains(featuringUser))
            throw new DoSvyaziMusicException("This featuring user already exists.");
        
        _featuring.Add(featuringUser);
    }

    public void DeleteFeaturingUser(MusicUser featuringUser)
    {
        _featuring.ThrowIfNull();
        
        var userToDelete = _featuring.FirstOrDefault(user => user.Id == featuringUser.Id)
            .ThrowIfNull(new EntityNotFoundException(nameof(MusicUser)));

        _featuring.Remove(userToDelete);
    }

    public bool Equals(Song? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as Song);
    public override int GetHashCode() => Id.GetHashCode();
}