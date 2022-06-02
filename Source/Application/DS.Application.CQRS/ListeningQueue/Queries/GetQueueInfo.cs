using DS.Application.DTO.ListeningQueue;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Queries;

public static class GetQueueInfo
{
    public record Query(Guid UserId) : IRequest<Response>;

    public record Response(ListeningQueueInfoDto QueueInfo);

    public class Handler : IRequestHandler<Query, Response>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var musicUser = await _context.MusicUsers.FindAsync(request.UserId);
            if (musicUser is null)
                throw new EntityNotFoundException("Music user cannot be found in the database");

            var listeningQueue = await _context.ListeningQueues.FindAsync(musicUser.ListeningQueue.Id);
            if (listeningQueue is null)
                throw new EntityNotFoundException($"Music user's {musicUser.ListeningQueue.Id} queue does not exist");

            var songsIds = listeningQueue.Songs.Select(song => song.Id);
            var queueDto = new ListeningQueueInfoDto(request.UserId, songsIds);

            return new Response(queueDto);
        }
    }
}