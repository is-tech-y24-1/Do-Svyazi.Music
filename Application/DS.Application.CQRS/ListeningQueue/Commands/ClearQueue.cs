using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class ClearQueue
{
    public record Command(Guid UserId) : IRequest;

    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}