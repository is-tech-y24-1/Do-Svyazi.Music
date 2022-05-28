using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class MediaLibrary : IEquatable<MediaLibrary>
{
    private List<Song> _songs;
    private List<Song> _authoredSongs;
    // private List<Playlist> _playlists TODO: uncomment when ready
    // private List<Playlist> _authoredPlaylists TODO: uncomment when ready
    
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
        // _playlists = new List<Playlist>(); TODO: uncomment when ready
        // _authoredPlaylists = new List<Playlist>(); TODO: uncomment when ready
    }
    public Guid Id { get; private init; }
    public MusicUser Owner { get; private init; }
    public IReadOnlyCollection<Song> GetSongs() => _songs;
    public IReadOnlyCollection<Song> GetAuthoredSongs() => _authoredSongs;
    // public IReadOnlyCollection<Song> GetPlaylist => _playlists; TODO: uncomment when ready
    // public IReadOnlyCollection<Song> GetAuthoredPlaylists => _authoredPlaylists; TODO: uncomment when ready

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
    
    // TODO: AddPlaylist, RemovePlaylist

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
        if (!song.Author.Equals(Owner))
            throw new DoSvyaziMusicException(ExceptionMessages.SongAccessForbidden);

        if (!_authoredSongs.Remove(song))
            throw new EntityNotFoundException(nameof(Song));
    }
    
    // TODO: Create / Delete AuthoredPlaylist
    public bool Equals(MediaLibrary? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as MediaLibrary);
    public override int GetHashCode() => Id.GetHashCode();
}