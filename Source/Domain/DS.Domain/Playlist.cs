using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class Playlist
{
    private readonly PlaylistSongs _songs;
    
    protected Playlist() {}

    public Playlist
    (
        string name,
        MusicUser author,
        PlaylistSongs songs,
        bool sharedForCommunity,
        string description = "",
        string coverUri = ""
    )
    {
        Name = name.ThrowIfNull();
        Author = author.ThrowIfNull();
        _songs = songs.ThrowIfNull();

        Description = description;
        Id = Guid.NewGuid();
        SharedForCommunity = sharedForCommunity;
        CoverUri = coverUri;
    }
    
    public Guid Id { get; private init; }
    public MusicUser Author { get; private init; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? CoverUri { get; set; }
    public bool SharedForCommunity { get; set; }
    public IReadOnlyCollection<Song> Songs => _songs;
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

    public void ChangeSongPosition(Song song, int newPosition)
    {
        song.ThrowIfNull();
        if (!_songs.Contains(song))
            throw new EntityNotFoundException(nameof(Song));
        
        _songs.Remove(song);
        var indexOfSongToInsertAfter = newPosition - 1;
        if (indexOfSongToInsertAfter + 1 == _songs.Count)
        {
            _songs.Add(song);
            return;
        }
        _songs.Insert(indexOfSongToInsertAfter + 1, song);
    }
}