using DS.Common.Exceptions;

namespace DS.Domain;

public class SongGenre : IEquatable<SongGenre>
{
#pragma warning disable CS8618
    protected SongGenre()
#pragma warning restore CS8618
    {
    }
    
    public SongGenre(string genreName)
    {
        if (string.IsNullOrWhiteSpace(genreName))
            throw new DoSvyaziMusicException("Music genre name can't be empty.");
        
        GenreName = genreName;
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; init; }
    public string GenreName { get; set; }

    public bool Equals(SongGenre? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as SongGenre);
    public override int GetHashCode() => Id.GetHashCode();
}