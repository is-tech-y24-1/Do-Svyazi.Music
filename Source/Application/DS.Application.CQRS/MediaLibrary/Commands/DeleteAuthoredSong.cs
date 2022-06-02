using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeleteAuthoredSong
{
    public record DeleteAuthoredSongCommand(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<DeleteAuthoredSongCommand>
    // {
    //     public async Task<Unit> Handle(DeleteAuthoredSongCommand request, CancellationToken cancellationToken) { }
    // }
}