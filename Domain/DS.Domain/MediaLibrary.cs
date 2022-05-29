using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class MediaLibrary : IEquatable<MediaLibrary>
{
    private List<Song> _songs;
    private List<Playlist> _playlists;

#pragma warning disable CS8618
    protected MediaLibrary() { }
#pragma warning restore CS8618
    
    public MediaLibrary(Guid ownerId)
    {
        if (ownerId == Guid.Empty)
            throw new GuidIsEmptyException(nameof(Guid));

        Id = ownerId;
        _songs = new List<Song>();
        _playlists = new List<Playlist>();
    }
    
    public Guid Id { get; private init; }
    public IReadOnlyCollection<Song> Songs => _songs;
    public IReadOnlyCollection<Song> AuthoredSongs => _songs.Where(s => s.Author.Id == Id).ToList();
    public IReadOnlyCollection<Playlist> Playlists => _playlists;
    public IReadOnlyCollection<Playlist> AuthoredPlaylists => _playlists.Where(p => p.Author.Id == Id).ToList();

    public void AddSong(Song song)
    {
        song.ThrowIfNull();
        if (_songs.Contains(song))
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
        if (_playlists.Contains(playlist))
            throw new DoSvyaziMusicException(ExceptionMessages.PlaylistAlreadyExists);

        _playlists.Add(playlist);
    }

    public void DeletePlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();
        if (!_playlists.Remove(playlist))
            throw new EntityNotFoundException(nameof(Playlist));
    }

    public bool Equals(MediaLibrary? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as MediaLibrary);
    public override int GetHashCode() => Id.GetHashCode();
}