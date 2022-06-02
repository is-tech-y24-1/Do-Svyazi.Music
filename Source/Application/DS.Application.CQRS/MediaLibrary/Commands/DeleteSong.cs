using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeleteSong
{
    public record DeleteSongCommand(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<DeleteSongCommand>
    // {
    //     public async Task<Unit> Handle(DeleteSongCommand request, CancellationToken cancellationToken) { }
    // }
}