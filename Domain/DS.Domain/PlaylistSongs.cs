﻿using System.Collections;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class PlaylistSongs : IList<Song>, IReadOnlyList<Song>
{
    private PlaylistSongNode? _head;
    private PlaylistSongNode? _tail;
    
    public PlaylistSongs(Song song)
    {
        song.ThrowIfNull();
        Count = 1;
        _head = new PlaylistSongNode(song);
        _tail = _head;
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
    
    public int Count { get; private set; }
    public bool IsReadOnly => false;

    public IEnumerator<Song> GetEnumerator()
    {
        if (_head is null) yield break;

        yield return _head.Song;
        var current = _head;
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
            _head = new PlaylistSongNode(item);
            _tail = _head;
            Count = 1;
            return;
        }

        Count++;
        _tail!.NextSongNode = new PlaylistSongNode(item);
        _tail = _tail.NextSongNode;
    }

    public void Clear()
    {
        Count = 0;
        _head = null;
        _tail = null;
    }

    public bool Contains(Song item)
    {
        item.ThrowIfNull();

        foreach (var playlistSong in this)
        {
            if (playlistSong.Equals(item))
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
        var current = _head!;

        while (current is not null)
        {
            if (current.Song.Equals(item))
            {
                if (current.Equals(_head!))
                    _head = _head!.NextSongNode;

                else if (current.Equals(_tail!))
                {
                    _tail = previous;
                    _tail!.NextSongNode = null;
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
            if (item.Equals(playlistSong))
                return index;

            index++;
        }

        return -1;
    }

    public void Insert(int index, Song item)
    {
        item.ThrowIfNull();
        
        if (index < 0 || index > Count)
            throw new DoSvyaziMusicException("Index of playlist songs is out of range!");

        if (index == Count)
        {
            Add(item);
            return;
        }
        
        PlaylistSongNode? previous = null;
        var current = _head!;
        
        for (var i = 0; i < index; i++)
        {
            previous = current;
            current = current!.NextSongNode;
        }

        var newNode = new PlaylistSongNode(item);
        newNode.NextSongNode = current;
        Count++;
        
        if (previous is not null)
        {
            previous.NextSongNode = newNode;
            return;
        }

        _head = newNode;
    }

    public void RemoveAt(int index)
    {
        if (index >= Count || index < 0)
            throw new DoSvyaziMusicException("Index of playlist songs is out of range!");
        
        PlaylistSongNode? previous = null;
        var current = _head!;

        for (var i = 0; i < index; i++)
        {
            previous = current;
            current = current!.NextSongNode;
        }
        
        if (current!.Equals(_head!))
            _head = _head!.NextSongNode;

        else if (current.Equals(_tail!))
        {
            _tail = previous;
            _tail!.NextSongNode = null;
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

        var current = _head;

        for (var i = 0; i < index; i++)
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
        
        var current = _head;

        for (var i = 0; i < index; i++)
        {
            current = current!.NextSongNode;
        }

        current!.Song = song;
    }
}