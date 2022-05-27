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

    public bool Equals(SongGenre? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id) && 
               GenreName == other.GenreName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SongGenre)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, GenreName);
    }
}