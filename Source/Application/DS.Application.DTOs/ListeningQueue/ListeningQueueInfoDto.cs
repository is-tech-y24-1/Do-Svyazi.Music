using DS.Application.DTO.Song;

namespace DS.Application.DTO.ListeningQueue;

public record ListeningQueueInfoDto
(
    Guid OwnerId, 
    IList<SongInfoDto> Songs
);