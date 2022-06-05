using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class AddSong
{
    public record AddSongCommand(Guid UserId, Guid SongId) : IRequest;

    public class Handler : IRequestHandler<AddSongCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddSongCommand request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);

            var musicUser = await _context.MusicUsers.FindAsync(request.UserId);
            if (musicUser is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            musicUser.MediaLibrary.AddSong(song);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}