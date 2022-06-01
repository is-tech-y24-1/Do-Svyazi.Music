using DS.Application.DTO.ListeningQueue;
using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Queries;

public static class GetQueueInfo
{
    public record Query(Guid UserId) : IRequest<Response>;

    public record Response(ListeningQueueInfoDto QueueInfo);

    // public class Handler : IRequestHandler<Query, Response>
    // {
    //     public async Task<Response> Handle(Query request, CancellationToken cancellationToken) { }
    // }
}