using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class DeleteFromQueue
{
    public record DeleteFromQueueCommand(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<DeleteFromQueueCommand>
    // {
    //     public async Task<Unit> Handle(DeleteFromQueueCommand request, CancellationToken cancellationToken) { }
    // }
}