using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class AddLastToQueue
{
    public record AddLastToQueueCommand(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<AddLastToQueueCommand>
    // {
    //     public async Task<Unit> Handle(AddLastToQueueCommand request, CancellationToken cancellationToken) { }
    // }
}