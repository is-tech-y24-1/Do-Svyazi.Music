using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class ChangeQueueSongPosition
{
    public record ChangeQueueSongPositionCommand(Guid UserId, Guid SongId, int NewPosition) : IRequest;

    // public class Handler : IRequestHandler<ChangeQueueSongPositionCommand>
    // {
    //     public async Task<Unit> Handle(ChangeQueueSongPositionCommand request, CancellationToken cancellationToken) { }
    // }
}