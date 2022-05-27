namespace DS.Domain;

// TODO: Implement this stub
public class MusicUser : IEquatable<MusicUser>
{
    public MusicUser()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; private init; }

    public bool Equals(MusicUser? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MusicUser)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}