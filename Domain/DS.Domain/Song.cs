using DS.Common.Exceptions;
using DS.Common.Extensions;
using DS.Domain.Types;

namespace DS.Domain;

public class Song
{
    private List<MusicUser> _featuring = new ();
    
    #pragma warning disable CS8618
    protected Song() { }
    #pragma warning restore CS8618

    public Song(AuthoredSongType type)
    {
        type.ThrowIfNull();
        
        Name = type.Name.ThrowIfNull();
        Genre = type.Genre.ThrowIfNull();
        Author = type.Author.ThrowIfNull();
        SongContentUri = type.SongContentUri.ThrowIfNull();

        if (string.IsNullOrWhiteSpace(type.Name))
            throw new DoSvyaziMusicException("Name cannot be empty");
        if (string.IsNullOrWhiteSpace(type.SongContentUri))
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
        _featuring.Add(featuringUser);
    }

    public void DeleteFeaturingUser(MusicUser featuringUser)
    {
        _featuring.ThrowIfNull();
        
        var userToDelete = _featuring.SingleOrDefault(user => user.Id == featuringUser.Id)
            .ThrowIfNull(new EntityNotFoundException(nameof(MusicUser)));

        _featuring.Remove(userToDelete);
    }
}