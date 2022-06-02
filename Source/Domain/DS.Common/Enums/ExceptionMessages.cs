namespace DS.Common.Enums;

public static class ExceptionMessages
{
    public static string SongAlreadyExists => "This song already exists";
    public static string SongAccessForbidden => "You have no access to this song.";

    public static string PlaylistAlreadyExists => "This playlist already exists";
    public static string PlaylistAccessForbidden => "You have no access to this playlist";
    public static string SongCannotBeFound => "Song cannot be found in the database";
    public static string UserCannotBeFound => "Music user cannot be found in the database";
    public static string ListeningQueueCannotBeFound => "ListeningQueue does not exist";
    public static string PlaylistCannotBeFound => "Playlist cannot be found in the database";
    public static string NoSongsInPlaylist => "There are no songs in playlist";
    public static string GenreCannotBeFound => "Genre cannot be found";
    public static string PlaylistModificationForbidden => "Playlist modification forbidden";
}