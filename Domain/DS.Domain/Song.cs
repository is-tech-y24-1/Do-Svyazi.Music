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

    public bool Equals(Song? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _featuring.Equals(other._featuring) &&
               Id.Equals(other.Id) && 
               Name == other.Name &&
               Genre.Equals(other.Genre) && 
               Author.Equals(other.Author) &&
               CoverUri == other.CoverUri &&
               SharedForCommunity == other.SharedForCommunity &&
               SongContentUri == other.SongContentUri;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Song)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_featuring, Id, Name, Genre, Author, CoverUri, SharedForCommunity, SongContentUri);
    }
}