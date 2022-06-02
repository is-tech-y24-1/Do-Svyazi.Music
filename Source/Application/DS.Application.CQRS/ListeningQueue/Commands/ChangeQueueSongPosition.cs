using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class ChangeQueueSongPosition
{
    public record Command(Guid UserId, Guid SongId, int NewPosition) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private MusicDbContext _context;

        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            var musicUser = await _context.MusicUsers.FindAsync(request.UserId);
            if (musicUser is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var listeningQueue = await _context.ListeningQueues.FindAsync(musicUser.ListeningQueue.Id);
            if (listeningQueue is null)
                throw new EntityNotFoundException(ExceptionMessages.ListeningQueueCannotBeFound);

            listeningQueue.ChangeSongPosition(song, request.NewPosition);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}