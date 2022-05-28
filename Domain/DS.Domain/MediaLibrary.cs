using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class MediaLibrary : IEquatable<MediaLibrary>
{
    private List<Song> _songs;
    private List<Song> _authoredSongs;
    private List<Playlist> _playlists;
    private List<Playlist> _authoredPlaylists;
    
#pragma warning disable CS8618
    protected MediaLibrary() { }
#pragma warning restore CS8618
    
    public MediaLibrary(MusicUser owner)
    {
        owner.ThrowIfNull();
        
        Id = Guid.NewGuid();
        Owner = owner;
        _songs = new List<Song>();
        _authoredSongs = new List<Song>();
        _playlists = new List<Playlist>();
        _authoredPlaylists = new List<Playlist>();
    }
    public Guid Id { get; private init; }
    public MusicUser Owner { get; private init; }
    public IReadOnlyCollection<Song> GetSongs() => _songs;
    public IReadOnlyCollection<Song> GetAuthoredSongs() => _authoredSongs;
    public IReadOnlyCollection<Playlist> GetPlaylist => _playlists;
    public IReadOnlyCollection<Playlist> GetAuthoredPlaylists => _authoredPlaylists;

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

    public void RemovePlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();
        if (!_playlists.Remove(playlist))
            throw new EntityNotFoundException(nameof(Playlist));
    }

    public void CreateAuthoredSong(Song song)
    {
        song.ThrowIfNull();
        if (_authoredSongs.Contains(song))
            throw new DoSvyaziMusicException(ExceptionMessages.SongAlreadyExists);
        
        _authoredSongs.Add(song);
    }

    public void DeleteAuthoredSong(Song song)
    {
        song.ThrowIfNull();

        if (!_authoredSongs.Remove(song))
            throw new EntityNotFoundException(nameof(Song));
    }
    
    public void CreateAuthoredPlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();
        if (_authoredPlaylists.Contains(playlist))
            throw new DoSvyaziMusicException(ExceptionMessages.SongAlreadyExists);
        
        _authoredPlaylists.Add(playlist);
    }

    public void DeleteAuthoredPlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();

        if (!_authoredPlaylists.Remove(playlist))
            throw new EntityNotFoundException(nameof(Playlist));
    }
    
    public bool Equals(MediaLibrary? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as MediaLibrary);
    public override int GetHashCode() => Id.GetHashCode();
}