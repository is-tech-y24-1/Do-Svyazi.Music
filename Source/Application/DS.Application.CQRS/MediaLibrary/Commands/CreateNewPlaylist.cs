using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewPlaylist
{
    public record CreateNewPlaylistCommand(Guid UserId, PlaylistCreationInfoDto PlaylistCreationInfo) : IRequest;

    // public class Handler : IRequestHandler<CreateNewPlaylistCommand>
    // {
    //     public async Task<Unit> Handle(CreateNewPlaylistCommand request, CancellationToken cancellationToken) { }
    // }
}