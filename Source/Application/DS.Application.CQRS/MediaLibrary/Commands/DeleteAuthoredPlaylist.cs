using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeleteAuthoredPlaylist
{
    public record DeleteAuthoredPlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;
    
    // public class Handler : IRequestHandler<DeleteAuthoredPlaylistCommand>
    // {
    //     public async Task<Unit> Handle(DeleteAuthoredPlaylistCommand request, CancellationToken cancellationToken) { }
    // }
}