using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class Song : IEquatable<Song>
{
    private readonly List<MusicUser> _featuring = new ();
    
    #pragma warning disable CS0169
    private readonly List<MediaLibrary> _addedToLibraries;
    #pragma warning restore CS0169

    protected Song() { }

    public Song
    (
        string name,
        SongGenre genre,
        MusicUser author,
        string songContentUri,
        string? coverUri = null
    )
    {
        Name = name.ThrowIfNull();
        Genre = genre.ThrowIfNull();
        Author = author.ThrowIfNull();
        ContentUri = songContentUri.ThrowIfNull();
        SharedForCommunity = false;
        CoverUri = coverUri;
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; private init; }
    public string Name { get; set; }
    public SongGenre Genre { get; set; }
    public MusicUser Author { get; private init; }
    public IReadOnlyCollection<MusicUser> Featuring => _featuring.ToList();
    public string? CoverUri { get; set; }
    public bool SharedForCommunity { get; set; }
    public string ContentUri { get; private init; }

    public void AddFeaturingUser(MusicUser featuringUser)
    {
        featuringUser.ThrowIfNull();
        if (Featuring.Contains(featuringUser))
            throw new DoSvyaziMusicException("This featuring user already exists.");
        
        _featuring.Add(featuringUser);
    }

    public void DeleteFeaturingUser(MusicUser featuringUser)
    {
        featuringUser.ThrowIfNull();
        
        if (!_featuring.Remove(featuringUser))
            throw new EntityNotFoundException(nameof(MusicUser));
    }

    public bool Equals(Song? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as Song);
    public override int GetHashCode() => Id.GetHashCode();
}