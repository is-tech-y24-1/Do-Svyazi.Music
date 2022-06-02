using DS.Application.DTO.ListeningQueue;
using DS.Application.DTO.Song;
using DS.Common.Enums;
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
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var listeningQueue = await _context.ListeningQueues.FindAsync(musicUser.ListeningQueue.Id);
            if (listeningQueue is null)
                throw new EntityNotFoundException(ExceptionMessages.ListeningQueueCannotBeFound);

            var songsDtos = new List<SongInfoDto>(listeningQueue.Songs.Count);
            foreach (var song in listeningQueue.Songs)
            {
                var songDto = new SongInfoDto
                (
                    song.Name,
                    song.Genre.Name,
                    song.Author.Name,
                    song.ContentUri,
                    song.CoverUri
                );
                songsDtos.Add(songDto);
            }
            var queueDto = new ListeningQueueInfoDto(request.UserId, songsDtos);

            return new Response(queueDto);
        }
    }
}