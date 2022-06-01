using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class AddSong
{
    public record Command(Guid UserId, Guid SongId) : IRequest;

    // public class Handler : IRequestHandler<Command>
    // {
    //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) { }
    // }
}