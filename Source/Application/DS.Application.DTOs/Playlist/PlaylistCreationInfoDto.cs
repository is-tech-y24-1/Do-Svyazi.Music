﻿namespace DS.Application.DTO.Playlist;

public record PlaylistCreationInfoDto
(
    Guid AuthorId,
    string Name,
    string? Description,
    string? CoverUri,
    bool SharedForCommunity,
    IList<Guid> SongsIds
);