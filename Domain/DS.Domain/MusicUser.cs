namespace DS.Domain;

// TODO: Implement this stub
public class MusicUser : IEquatable<MusicUser>
{
    public MusicUser()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; private init; }

    public bool Equals(MusicUser? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as MusicUser);
    public override int GetHashCode() => Id.GetHashCode();
}