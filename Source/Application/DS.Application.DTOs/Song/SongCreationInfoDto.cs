namespace DS.Application.DTO.Song;

public record SongCreationInfoDto
(
    string Name,
    Guid GenreId,
    Guid AuthorId
);