using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class ChangeQueueSongPosition
{
    public record ChangePositionCommand(Guid UserId, Guid SongId, int NewPosition) : IRequest;

    // public class Handler : IRequestHandler<ChangePositionCommand>
    // {
    //     public async Task<Unit> Handle(ChangePositionCommand request, CancellationToken cancellationToken) { }
    // }
}