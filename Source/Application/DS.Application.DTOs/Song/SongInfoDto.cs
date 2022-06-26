using Microsoft.AspNetCore.Mvc;

namespace DS.Application.DTO.Song;

public record SongInfoDto
(
    string Name,
    string GenreName,
    string AuthorName
)
{
    public SongInfoDto() 
        : this(string.Empty, string.Empty, string.Empty) { }
};