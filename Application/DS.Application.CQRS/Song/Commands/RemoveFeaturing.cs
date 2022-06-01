using MediatR;

namespace DS.Application.CQRS.Song.Commands;

public static class RemoveFeaturing
{
    public record Command(Guid UserId, Guid SongId, Guid FeaturingUserId) : IRequest;
    
    // public class Handler : IRequestHandler<Command>
    // {
    //     public Task<Unit> Handle(Command request, CancellationToken cancellation) { }
    // }
}