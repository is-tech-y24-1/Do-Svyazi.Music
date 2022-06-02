using MediatR;

namespace DS.Application.CQRS.Song.Commands;

public static class AddFeaturing
{
    public record AddFeaturingCommand(Guid UserId, Guid SongId, Guid FeaturingUserId) : IRequest;
    
    // public class Handler : IRequestHandler<AddFeaturingCommand>
    // {
    //     public Task<Unit> Handle(AddFeaturingCommand request, CancellationToken cancellation) { }
    // }
}