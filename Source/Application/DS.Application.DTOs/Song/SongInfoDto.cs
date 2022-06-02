using DS.Application.DTO.MusicUser;

namespace DS.Application.DTO.Song;

public record SongInfoDto
(
    string Name, 
    string GenreName,
    string AuthorName,
    string ContentUri
);