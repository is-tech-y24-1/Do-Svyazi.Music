using DS.Common.Extensions;

namespace DS.Domain;

public class MediaLibraryPlaylist : IEquatable<MediaLibraryPlaylist>
{
    #pragma warning disable CS8618
    protected MediaLibraryPlaylist() { }
    #pragma warning restore CS8618

    public MediaLibraryPlaylist(MediaLibrary mediaLibrary, Playlist playlist)
    {
        MediaLibrary = mediaLibrary.ThrowIfNull();
        Playlist = playlist.ThrowIfNull();

        MediaLibraryId = mediaLibrary.Id;
        PlaylistId = playlist.Id;
    }
    
    public Guid MediaLibraryId { get; private init; }
    public Guid PlaylistId { get; private init; }
    
    public MediaLibrary MediaLibrary { get; set; }
    public Playlist Playlist { get; set; }
    
    public bool Equals(MediaLibraryPlaylist? other)
    {
        return other != null
               && MediaLibraryId == other.MediaLibraryId
               && PlaylistId == other.PlaylistId;
    }

    public override bool Equals(object? obj) => Equals(obj as MediaLibraryPlaylist);
    public override int GetHashCode() => HashCode.Combine(PlaylistId, MediaLibraryId);
}