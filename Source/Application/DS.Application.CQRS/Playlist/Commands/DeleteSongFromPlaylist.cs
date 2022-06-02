using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class DeleteSongFromPlaylist
{
    public record DeleteSongCommand(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<DeleteSongCommand>
    // {
    //     public async Task<Unit> Handle(DeleteSongCommand request, CancellationToken cancellationToken) { }
    // }
}