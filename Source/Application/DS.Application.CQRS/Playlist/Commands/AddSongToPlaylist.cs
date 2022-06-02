using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class AddSongToPlaylist
{
    public record AddSongCommand(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<AddSongCommand>
    // {
    //     public async Task<Unit> Handle(AddSongCommand request, CancellationToken cancellationToken) { }
    // }
}