using DS.Common.Extensions;

namespace DS.Domain;

public class FeaturingUser : IEquatable<FeaturingUser>
{
    #pragma warning disable CS8618
    protected FeaturingUser() { }
    #pragma warning restore CS8618

    public FeaturingUser(MusicUser user, Song song)
    {
        Song = song.ThrowIfNull();
        MusicUser = user.ThrowIfNull();

        SongId = song.Id;
        MusicUserId = user.Id;
    }
    
    public Guid MusicUserId { get; private init; }
    public Guid SongId { get; private init; }
    
    public MusicUser MusicUser { get; set; }
    public Song Song { get; set; }

    public bool Equals(FeaturingUser? other)
    {
        return other != null
               && SongId == other.SongId
               && MusicUserId == other.MusicUserId;
    }

    public override bool Equals(object? obj) => Equals(obj as FeaturingUser);
    public override int GetHashCode() => HashCode.Combine(MusicUserId, SongId);
}