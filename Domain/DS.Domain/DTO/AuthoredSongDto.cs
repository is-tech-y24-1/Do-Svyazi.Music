namespace DS.Domain.DTO;

public record AuthoredSongDto(string Name, SongGenre Genre, MusicUser Author, string SongContentUri);