using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class DeleteSongFromPlaylist
{
    public record Command(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}