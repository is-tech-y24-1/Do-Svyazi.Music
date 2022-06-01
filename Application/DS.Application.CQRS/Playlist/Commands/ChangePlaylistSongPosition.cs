using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class ChangePlaylistSongPosition
{
    public record Command(Guid UserId, Guid PlaylistId, Guid SongId, int NewPosition) : IRequest;

    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}