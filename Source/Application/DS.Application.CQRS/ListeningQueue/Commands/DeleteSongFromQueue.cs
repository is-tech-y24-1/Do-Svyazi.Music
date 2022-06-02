using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class DeleteFromQueue
{
    public record Command(Guid UserId, Guid SongId) : IRequest;

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
                throw new EntityNotFoundException("Song cannot be found in the database");
            
            var musicUser = await _context.MusicUsers.FindAsync(request.UserId);
            if (musicUser is null)
                throw new EntityNotFoundException("Music user cannot be found in the database");

            var listeningQueue = await _context.ListeningQueues.FindAsync(musicUser.ListeningQueue.Id);
            if (listeningQueue is null)
                throw new EntityNotFoundException($"Music user's {musicUser.ListeningQueue.Id} queue does not exist");

            listeningQueue.RemoveSong(song);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}