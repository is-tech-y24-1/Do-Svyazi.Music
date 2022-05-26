namespace DS.Domain.Types;

public record AuthoredSongType(string Name, SongGenre Genre, MusicUser Author, string SongContentUri);