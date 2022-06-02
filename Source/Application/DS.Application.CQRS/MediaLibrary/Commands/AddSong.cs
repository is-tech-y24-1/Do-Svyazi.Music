using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class AddSong
{
    public record AddSongCommand(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<AddSongCommand>
    // {
    //     public async Task<Unit> Handle(AddSongCommand request, CancellationToken cancellationToken) { }
    // }
}