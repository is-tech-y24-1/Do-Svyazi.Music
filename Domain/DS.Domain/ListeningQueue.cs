using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class ListeningQueue
{
    private readonly PlaylistSongs _songs = new();
    public ListeningQueue(Guid ownerId)
    {
        OwnerId = ownerId.ThrowIfNull();
    }
    public Guid OwnerId { get; init; }
    public IReadOnlyCollection<Song> Songs => _songs;

    public void AddNextSongToPlay(Song song)
    {
        song.ThrowIfNull();
        _songs.Insert(1, song);
    }

    public void AddLastToPlay(Song song)
    {
        song.ThrowIfNull();
        _songs.Add(song);
    }

    public void ChangeSongPosition(Song song, Song songToInsertAfter)
    {
        song.ThrowIfNull();
        songToInsertAfter.ThrowIfNull();

        if (!_songs.Contains(song))
            throw new DoSvyaziMusicException("Song to insert is not in the queue");
        if(!_songs.Contains(songToInsertAfter))
            throw new DoSvyaziMusicException("Song to insert after is not in the queue");

        _songs.Remove(song);
        var indexOfSongToInsertAfter = _songs.IndexOf(songToInsertAfter);
        if (indexOfSongToInsertAfter + 1 == _songs.Count)
        {
            _songs.Add(song);
            return;
        }
        _songs.Insert(indexOfSongToInsertAfter + 1, song);
    }

    public void RemoveSong(Song song)
    {
        song.ThrowIfNull();
        if (!_songs.Remove(song))
            throw new DoSvyaziMusicException("Song to delete is not in the queue");
    }

    public Song? GetCurrentPlaying() => _songs.ToList().FirstOrDefault();
}