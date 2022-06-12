using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class MediaLibrary : IEquatable<MediaLibrary>
{
    private readonly List<Song> _songs;
    private readonly List<Playlist> _playlists;

    protected MediaLibrary() {}
    
    public MediaLibrary(Guid ownerOwnerId)
    {
        if (ownerOwnerId == Guid.Empty)
            throw new GuidIsEmptyException(nameof(Guid));

        OwnerId = ownerOwnerId;
        _songs = new List<Song>();
        _playlists = new List<Playlist>();
    }
    
    public Guid OwnerId { get; private init; }
    public IReadOnlyCollection<Song> Songs => _songs.ToList();
    public IReadOnlyCollection<Song> AuthoredSongs => Songs.Where(s => s.Author.Id == OwnerId).ToList();
    public IReadOnlyCollection<Playlist> Playlists => _playlists.ToList();
    public IReadOnlyCollection<Playlist> AuthoredPlaylists => Playlists.Where(p => p.Author.Id == OwnerId).ToList();

    public void AddSong(Song song)
    {
        song.ThrowIfNull();
        if (Songs.Contains(song))
            throw new DoSvyaziMusicException(ExceptionMessages.SongAlreadyExists);

        _songs.Add(song);
    }

    public void DeleteSong(Song song)
    {
        song.ThrowIfNull();
        if (!_songs.Remove(song))
            throw new EntityNotFoundException(nameof(Song));
    }

    public void AddPlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();
        if (Playlists.Contains(playlist))
            throw new DoSvyaziMusicException(ExceptionMessages.PlaylistAlreadyExists);

        _playlists.Add(playlist);
    }

    public void DeletePlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();
        if (!_playlists.Remove(playlist))
            throw new EntityNotFoundException(nameof(Playlist));
    }

    public bool Equals(MediaLibrary? other) => other?.OwnerId.Equals(OwnerId) ?? false;
    public override bool Equals(object? obj) => Equals(obj as MediaLibrary);
    public override int GetHashCode() => OwnerId.GetHashCode();
}