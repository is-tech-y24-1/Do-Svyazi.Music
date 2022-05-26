using DS.Common.Extensions;

namespace DS.Domain;

public class PlaylistSongNode
{
    public PlaylistSongNode(Song song)
    {
        Song = song.ThrowIfNull();
        Id = Guid.NewGuid();
    }
    
    #pragma warning disable CS8618
    protected PlaylistSongNode() { }
    #pragma warning restore CS8618
    
    public Guid Id { get; private init; }
    public Song Song { get; set; }
    public PlaylistSongNode? NextSongNode { get; set; }
}