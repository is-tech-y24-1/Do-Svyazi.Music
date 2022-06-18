using Microsoft.AspNetCore.Http;

namespace DS.Application.DTO.MusicUser;

public record MusicUserCreationInfoDto
(
    Guid Id,
    string Name,
    IFormFile? ProfilePicture
);