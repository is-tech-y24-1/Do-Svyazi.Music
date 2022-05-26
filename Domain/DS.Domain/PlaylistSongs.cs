using System.Collections;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class PlaylistSongs : IList<Song>
{
    public PlaylistSongs(Song song)
    {
        song.ThrowIfNull();
        Count = 1;
        Head = new PlaylistSongNode(song);
        Tail = Head;
    }

    public PlaylistSongs(ICollection<Song> songs)
    {
        songs.ThrowIfNull();
        Count = 0;
        foreach (var song in songs)
            Add(song);
    }

    public PlaylistSongs()
    {
        Count = 0;
    }

    private PlaylistSongNode? Head { get; set; }
    private PlaylistSongNode? Tail { get; set; }
    public int Count { get; private set; }
    public bool IsReadOnly => false;

    public IEnumerator<Song> GetEnumerator()
    {
        if (Head is null) yield break;

        yield return Head.Song;
        var current = Head;
        while (current.NextSongNode is not null)
        {
            current = current.NextSongNode;
            yield return current.Song;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Song item)
    {
        item.ThrowIfNull();
        
        if (Count == 0)
        {
            Head = new PlaylistSongNode(item);
            Tail = Head;
            Count = 1;
            return;
        }

        Count++;
        Tail!.NextSongNode = new PlaylistSongNode(item);
        Tail = Tail.NextSongNode;
    }

    public void Clear()
    {
        Count = 0;
        Head = null;
        Tail = null;
    }

    public bool Contains(Song item)
    {
        item.ThrowIfNull();

        foreach (var playlistSong in this)
        {
            if (playlistSong.Id == item.Id)
                return true;
        }

        return false;
    }

    public void CopyTo(Song[] array, int arrayIndex)
    {
        foreach (var playlistSong in this)
            array[arrayIndex++] = playlistSong;
    }

    public bool Remove(Song item)
    {
        item.ThrowIfNull();
        if (Count == 0) return false;
        PlaylistSongNode? previous = null;
        var current = Head!;

        while (current is not null)
        {
            if (current.Song.Id == item.Id)
            {
                if (current.Id == Head!.Id)
                    Head = Head.NextSongNode;

                else if (current.Id == Tail!.Id)
                {
                    Tail = previous;
                    Tail!.NextSongNode = null;
                }

                else
                    previous!.NextSongNode = current.NextSongNode;

                Count--;
                return true;
            }
            
            previous = current;
            current = current.NextSongNode;
        }

        return false;
    }
    
    public int IndexOf(Song item)
    {
        item.ThrowIfNull();
        var index = 0;
        
        foreach (var playlistSong in this)
        {
            if (item.Id == playlistSong.Id)
                return index;

            index++;
        }

        return -1;
    }

    public void Insert(int index, Song item)
    {
        item.ThrowIfNull();
        
        if (index < 0 || index >= Count)
            throw new DoSvyaziMusicException("Index of playlist songs is out of range!");
        
        PlaylistSongNode? previous = null;
        var current = Head!;
        
        for (var i = 0; i <= index; i++)
        {
            previous = current;
            current = current!.NextSongNode;
        }

        var newNode = new PlaylistSongNode(item);
        previous!.NextSongNode = newNode;
        newNode.NextSongNode = current;
        Count++;
    }

    public void RemoveAt(int index)
    {
        if (index >= Count || index < 0)
            throw new DoSvyaziMusicException("Index of playlist songs is out of range!");
        
        PlaylistSongNode? previous = null;
        var current = Head!;

        for (var i = 0; i <= index; i++)
        {
            previous = current;
            current = current!.NextSongNode;
        }
        
        if (current!.Id == Head!.Id)
            Head = Head.NextSongNode;

        else if (current.Id == Tail!.Id)
        {
            Tail = previous;
            Tail!.NextSongNode = null;
        }

        else
            previous!.NextSongNode = current.NextSongNode;

        Count--;
    }

    public Song this[int index]
    {
        get => GetAt(index);
        set => SetAt(index, value);
    }
    
    private Song GetAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new DoSvyaziMusicException("Index of playlist songs is out of range!");

        var current = Head;

        for (var i = 0; i <= index; i++)
        {
            current = current!.NextSongNode;
        }

        return current!.Song;
    }
    
    private void SetAt(int index, Song song)
    {
        song.ThrowIfNull();
        
        if (index < 0 || index >= Count)
            throw new DoSvyaziMusicException("Index of playlist songs is out of range!");
        
        var current = Head;

        for (var i = 0; i <= index; i++)
        {
            current = current!.NextSongNode;
        }

        current!.Song = song;
    }
}