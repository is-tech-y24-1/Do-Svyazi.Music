using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class ChangePlaylistSongPosition
{
    public record ChangePositionCommand(Guid UserId, Guid PlaylistId, Guid SongId, int NewPosition) : IRequest;

    // public class Handler : IRequestHandler<ChangePositionCommand>
    // {
    //     public async Task<Unit> Handle(ChangePositionCommand request, CancellationToken cancellationToken) { }
    // }
}