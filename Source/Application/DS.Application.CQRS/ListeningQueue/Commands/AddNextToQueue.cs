using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class AddNextToQueue
{
    public record AddNextToQueueCommand(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<AddNextToQueueCommand>
    // {
    //     public async Task<Unit> Handle(AddNextToQueueCommand request, CancellationToken cancellationToken) { }
    // }
}