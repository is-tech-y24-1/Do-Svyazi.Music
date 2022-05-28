using DS.Common.Extensions;

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
        GenreName = genreName.ThrowIfNull();
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; init; }
    public string GenreName { get; set; }

    public bool Equals(SongGenre? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as SongGenre);
    public override int GetHashCode() => Id.GetHashCode();
}