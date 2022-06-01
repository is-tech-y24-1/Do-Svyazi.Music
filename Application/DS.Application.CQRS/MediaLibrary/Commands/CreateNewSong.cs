using DS.Application.DTO.Song;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewSong
{
    public record Command(Guid UserId, SongCreationInfoDto SongCreationInfo) : IRequest;

    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}