using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class AddLastToQueue
{
    public record Command(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}