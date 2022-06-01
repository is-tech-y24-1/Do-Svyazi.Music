namespace DS.Application.DTO.ListeningQueue;

public record ListeningQueueInfoDto(Guid OwnerId, IEnumerable<Guid> SongsIds);