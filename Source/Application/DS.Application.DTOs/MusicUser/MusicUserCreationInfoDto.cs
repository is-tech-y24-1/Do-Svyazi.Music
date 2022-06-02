namespace DS.Application.DTO.MusicUser;

public record MusicUserCreationInfoDto
(
    Guid Id,
    string Name,
    string? ProfilePictureUri
);