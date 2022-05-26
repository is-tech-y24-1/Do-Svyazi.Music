using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.Common.Extensions;
using DS.Domain.DTO;

namespace DS.Domain;

public class MediaLibrary
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

    public Song AddSong(Song song)
    {
        song.ThrowIfNull();
        if (_songs.Any(s => s.Id == song.Id))
            throw new DoSvyaziMusicException(ExceptionMessages.SongAlreadyExists);

        _songs.Add(song);
        return song;
    }

    public void DeleteSong(Song song)
    {
        song.ThrowIfNull();
        if (_songs.All(s => s.Id != song.Id))
            throw new EntityNotFoundException(nameof(Song));

        _songs.Remove(song);
    }
    
    // TODO: AddPlaylist, RemovePlaylist

    public Song CreateAuthoredSong(AuthoredSongDto dto)
    {
        dto.ThrowIfNull();
        var song = new Song(dto);
        if (_authoredSongs.Any(s => s.Id == song.Id))
            throw new DoSvyaziMusicException(ExceptionMessages.SongAlreadyExists);
        
        _authoredSongs.Add(song);
        
        return song;
    }

    public void DeleteAuthoredSong(Song song)
    {
        song.ThrowIfNull();
        if (song.Author != Owner)
            throw new DoSvyaziMusicException(ExceptionMessages.SongAccessForbidden);

        if (_authoredSongs.Any(s => s.Id != song.Id))
            throw new EntityNotFoundException(nameof(Song));
        
        _authoredSongs.Remove(song);
    }
    
    // TODO: Create / Delete AuthoredPlaylist
}