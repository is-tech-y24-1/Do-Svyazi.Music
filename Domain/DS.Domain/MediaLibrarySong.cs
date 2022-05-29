using DS.Common.Extensions;

namespace DS.Domain;

public class MediaLibrarySong : IEquatable<MediaLibrarySong>
{
    #pragma warning disable CS8618
    protected MediaLibrarySong() { }
    #pragma warning restore CS8618

    public MediaLibrarySong(MediaLibrary mediaLibrary, Song song)
    {
        MediaLibrary = mediaLibrary.ThrowIfNull();
        Song = song.ThrowIfNull();

        MediaLibraryId = mediaLibrary.Id;
        SongId = song.Id;
    }
    
    public Guid MediaLibraryId { get; private init; }
    public Guid SongId { get; private init; }
    
    public MediaLibrary MediaLibrary { get; set; }
    public Song Song { get; set; }
    
    public bool Equals(MediaLibrarySong? other)
    {
        return other != null
               && SongId == other.SongId
               && MediaLibraryId == other.MediaLibraryId;
    }

    public override bool Equals(object? obj) => Equals(obj as MediaLibrarySong);
    public override int GetHashCode() => HashCode.Combine(MediaLibraryId, SongId);
}