﻿using System.Collections;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class PlaylistSong : IEnumerable<Song>
{
    public PlaylistSong(Song song)
    {
        Song = song.ThrowIfNull();
        Id = Guid.NewGuid();
    }
    
    #pragma warning disable CS8618
    protected  PlaylistSong() { }
    #pragma warning restore CS8618
    
    public Guid Id { get; private init; }
    public Song Song { get; set; }
    public PlaylistSong? PreviousSong { get; set; }
    public PlaylistSong? NextSong { get; set; }
    
    public IEnumerator<Song> GetEnumerator()
    {
        var current = this;
        yield return Song;

        while (current.NextSong is not null)
        {
            current = current.NextSong;
            yield return current.Song;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddLast(Song song)
    {
        var lastSong = this;
        while (lastSong.NextSong is not null)
            lastSong = lastSong.NextSong;

        var newSong = new PlaylistSong(song);
        TieUpEnds(lastSong, newSong);
    }

    public void AddFirst(Song song)
    {
        song.ThrowIfNull();
        var firstSong = Song;
        Song = song;
        var newSecondElement = new PlaylistSong(firstSong);
        InsertAfter(newSecondElement, this);
    }

    public bool Contains(Song song)
    {
        song.ThrowIfNull();
        
        foreach (var songInPlaylist in this)
        {
            if (songInPlaylist.Id == song.Id)
                return true;
        }

        return false;
    }

    public void InsertAfter(Song songToInsert, Song songAfter)
    {
        songToInsert.ThrowIfNull();
        songAfter.ThrowIfNull();
        var found = FindPlaylistSong(songAfter);

        if (found is null)
            throw new EntityNotFoundException($"Song to insert after (with Id: {songAfter.Id}) not found");

        var newSong = new PlaylistSong(songToInsert);
        InsertAfter(newSong, found);
    }

    public bool Remove(Song song)
    {
        song.ThrowIfNull();
        var found = FindPlaylistSong(song);
        if (found is null) return false;
        
        if (found.PreviousSong is null) // removing first song
        {
            if (found.NextSong is null) // trying to remove the only song in playlist
                throw new DoSvyaziMusicException("Impossible to have playlist without songs");

            found.Song = found.NextSong.Song;
            var thirdSong = found.NextSong.NextSong;
            found.NextSong.Isolate();
            
            if (thirdSong is null)
            {
                found.NextSong = null;
                return true;
            }
            
            TieUpEnds(found, thirdSong);
            return true;
        }

        if (found.NextSong is null) // removing last song
        {
            found.PreviousSong.NextSong = null;
            found.Isolate();
            return true;
        }
        
        TieUpEnds(found.PreviousSong, found.NextSong);
        found.Isolate();
        return true;
    }

    public Song? FindSong(Song song)
    {
        song.ThrowIfNull();
        
        foreach (var songInPlaylist in this)
        {
            if (song.Id == songInPlaylist.Id)
                return songInPlaylist;
        }

        return null;
    }
    
    private PlaylistSong? FindPlaylistSong(Song song)
    {
        var current = this;
        
        while (current is not null)
        {
            if (current.Song.Id == song.Id)
                return current;

            current = current.NextSong;
        }

        return null;
    }

    private void TieUpEnds(PlaylistSong head, PlaylistSong tail)
    {
        head.NextSong = tail;
        tail.PreviousSong = head;
    }

    private void InsertAfter(PlaylistSong songToInsert, PlaylistSong songAfter)
    {
        var thirdSong = songAfter.NextSong;
        TieUpEnds(songAfter, songToInsert);
        
        if (thirdSong is null) return;
        TieUpEnds(songToInsert, thirdSong);
    }

    private void Isolate()
    {
        NextSong = null;
        PreviousSong = null;
    }
}