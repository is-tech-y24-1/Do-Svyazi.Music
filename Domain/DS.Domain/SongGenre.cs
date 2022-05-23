using DS.Common.Exceptions;

namespace DS.Domain;

/**
 * Describes song genre in Entity Framework.
 */
public class SongGenre
{
    public SongGenre(string genreName)
    {
        if (string.IsNullOrEmpty(genreName))
            throw new DoSvyaziMusicException("Music genre name can't be empty.");
        
        GenreName = genreName;
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; }
    public string GenreName { get; set; }
}