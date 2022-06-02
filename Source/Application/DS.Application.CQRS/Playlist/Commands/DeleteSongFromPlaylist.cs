using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class DeleteSongFromPlaylist
{
    public record DeletePlaylistSongCommand(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<DeletePlaylistSongCommand>
    // {
    //     public async Task<Unit> Handle(DeletePlaylistSongCommand request, CancellationToken cancellationToken) { }
    // }
}