using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class AddSongToPlaylist
{
    public record AddPlaylistSongCommand(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<AddPlaylistSongCommand>
    // {
    //     public async Task<Unit> Handle(AddPlaylistSongCommand request, CancellationToken cancellationToken) { }
    // }
}