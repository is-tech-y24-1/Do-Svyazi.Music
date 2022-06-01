using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class ChangeQueueSongPosition
{
    public record Command(Guid UserId, Guid SongId, int NewPosition) : IRequest;

    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}