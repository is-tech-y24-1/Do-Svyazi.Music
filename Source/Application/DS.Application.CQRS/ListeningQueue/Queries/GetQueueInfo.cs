using DS.Application.DTO.ListeningQueue;
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
            var listeningQueue = await _context.ListeningQueues.FindAsync(musicUser?.ListeningQueue.Id);
            var songsIds = listeningQueue?.Songs.Select(song => song.Id);
            var queueDto = new ListeningQueueInfoDto(request.UserId, songsIds!);

            return new Response(queueDto);
        }
    }
}