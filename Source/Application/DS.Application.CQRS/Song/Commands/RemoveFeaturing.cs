using MediatR;

namespace DS.Application.CQRS.Song.Commands;

public static class RemoveFeaturing
{
    public record RemoveFeaturingCommand(Guid UserId, Guid SongId, Guid FeaturingUserId) : IRequest;
    
    // public class Handler : IRequestHandler<RemoveFeaturingCommand>
    // {
    //     public Task<Unit> Handle(RemoveFeaturingCommand request, CancellationToken cancellation) { }
    // }
}