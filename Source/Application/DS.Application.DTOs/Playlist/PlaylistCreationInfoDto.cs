using Microsoft.AspNetCore.Http;

namespace DS.Application.DTO.Playlist;

public record PlaylistCreationInfoDto
(
    Guid AuthorId,
    string Name,
    string? Description,
    bool SharedForCommunity,
    IList<Guid> SongsIds,
    IFormFile? Cover
);