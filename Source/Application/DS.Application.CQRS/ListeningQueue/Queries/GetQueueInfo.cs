using AutoMapper;
using DS.Application.DTO.ListeningQueue;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Queries;

public static class GetQueueInfo
{
    public record GetInfoQuery(Guid UserId) : IRequest<Response>;

    public record Response(ListeningQueueInfoDto QueueInfo);
    
    public class Handler : IRequestHandler<GetInfoQuery, Response>
    {
        private MusicDbContext _context;
        private IMapper _mapper;
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken)
        {
            var musicUser = await _context.MusicUsers.FindAsync(request.UserId);
            if (musicUser is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var listeningQueue = await _context.ListeningQueues.FindAsync(musicUser.ListeningQueue.OwnerId);
            if (listeningQueue is null)
                throw new EntityNotFoundException(ExceptionMessages.ListeningQueueCannotBeFound);

            return new Response(_mapper.Map<ListeningQueueInfoDto>(listeningQueue));
        }
    }
}