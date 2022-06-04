using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class ListeningQueue
{
    private readonly PlaylistSongs _songs = new();
    public ListeningQueue(Guid ownerId)
    {
        OwnerId = ownerId;
    }
    
    protected ListeningQueue() {}
    
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

    public void ChangeSongPosition(Song song, int newPosition)
    {
        song.ThrowIfNull();

        if (!_songs.Contains(song))
            throw new DoSvyaziMusicException("Song to insert is not in the queue");
        if(newPosition < 0 || newPosition > _songs.Count)
            throw new DoSvyaziMusicException("There is no such position in the queue");

        _songs.Remove(song);
        if (newPosition == _songs.Count)
        {
            _songs.Add(song);
            return;
        }
        _songs.Insert(newPosition, song);
    }

    public void RemoveSong(Song song)
    {
        song.ThrowIfNull();
        if (!_songs.Remove(song))
            throw new DoSvyaziMusicException("Song to delete is not in the queue");
    }

    public void Clear() => _songs.Clear();

    public Song? GetCurrentPlaying() => _songs.ToList().FirstOrDefault();
}