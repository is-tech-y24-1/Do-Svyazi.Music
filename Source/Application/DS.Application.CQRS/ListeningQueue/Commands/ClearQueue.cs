using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class ClearQueue
{
    public record ClearQueueCommand(Guid UserId) : IRequest;

    // public class Handler : IRequestHandler<ClearQueueCommand>
    // {
    //     public async Task<Unit> Handle(ClearQueueCommand request, CancellationToken cancellationToken) { }
    // }
}