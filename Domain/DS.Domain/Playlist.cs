using DS.Common.Extensions;

namespace DS.Domain;

public class Playlist
{
#pragma warning disable CS8618
    protected Playlist()
    { }
#pragma warning restore CS8618

    public Playlist(
        string name, 
        MusicUser author,
        PlaylistSongs songs, 
        bool sharedForCommunity,
        string description = "", 
        string coverUri = "")
    {
        Name = name.ThrowIfNull();
        Author = author.ThrowIfNull();
        Songs = songs.ThrowIfNull();

        Description = description;
        id = Guid.NewGuid();
        SharedForCommunity = sharedForCommunity;
        CoverUri = coverUri;
    }
    
    public Guid id { get; private init; }
    public MusicUser Author { get; private init; }
    public PlaylistSongs Songs { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CoverUri { get; set; }
    public bool SharedForCommunity { get; set; }
    public IReadOnlyCollection<Song> GetAll() => Songs;
    
    public void AddSong(Song song)
    {
        song.ThrowIfNull();
        Songs.Add(song);
    }

    public void DeleteSong(Song song)
    {
        song.ThrowIfNull();
        Songs.Remove(song);
    }

    public void ChangeSongPosition(Song song, int newPosition)
    {
        song.ThrowIfNull();
        Songs.Insert(newPosition, song);
    }
}