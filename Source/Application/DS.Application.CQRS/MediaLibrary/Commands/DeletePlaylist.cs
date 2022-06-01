using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeletePlaylist
{
    public record Command(Guid UserId, Guid PlaylistId) : IRequest;
    
    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}