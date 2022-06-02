using DS.Application.DTO.Song;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewSong
{
    public record CreateNewSongCommand(Guid UserId, SongCreationInfoDto SongCreationInfo) : IRequest;

    // public class Handler : IRequestHandler<CreateNewSongCommand>
    // {
    //     public async Task<Unit> Handle(CreateNewSongCommand request, CancellationToken cancellationToken) { }
    // }
}