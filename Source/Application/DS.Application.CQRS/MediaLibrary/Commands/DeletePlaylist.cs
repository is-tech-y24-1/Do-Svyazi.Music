using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeletePlaylist
{
    public record DeletePlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;
    
    // public class Handler : IRequestHandler<DeletePlaylistCommand>
    // {
    //     public async Task<Unit> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken) { }
    // }
}