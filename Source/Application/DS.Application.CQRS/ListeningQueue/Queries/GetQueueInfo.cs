using DS.Application.DTO.ListeningQueue;
using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Queries;

public static class GetQueueInfo
{
    public record GetInfoQuery(Guid UserId) : IRequest<Response>;

    public record Response(ListeningQueueInfoDto QueueInfo);

    // public class Handler : IRequestHandler<GetInfoQuery, Response>
    // {
    //     public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken) { }
    // }
}