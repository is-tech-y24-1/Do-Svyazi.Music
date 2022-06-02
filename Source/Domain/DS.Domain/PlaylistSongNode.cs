using DS.Common.Extensions;

namespace DS.Domain;

public class PlaylistSongNode : IEquatable<PlaylistSongNode>
{
    public PlaylistSongNode(Song song)
    {
        Song = song.ThrowIfNull();
        Id = Guid.NewGuid();
    }
    
    protected PlaylistSongNode() {}
    
    public Guid Id { get; private init; }
    public Song Song { get; set; }
    public PlaylistSongNode? NextSongNode { get; set; }

    public bool Equals(PlaylistSongNode? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as PlaylistSongNode);
    public override int GetHashCode() => Id.GetHashCode();
}