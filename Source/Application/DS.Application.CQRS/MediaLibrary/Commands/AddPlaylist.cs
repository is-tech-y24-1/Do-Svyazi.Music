using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class AddPlaylist
{
    public record AddPlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;

    // public class Handler : IRequestHandler<AddPlaylistCommand>
    // {
    //     public async Task<Unit> Handle(AddPlaylistCommand request, CancellationToken cancellationToken) { }
    // }
}