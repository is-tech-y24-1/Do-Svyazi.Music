using DS.Application.DTO.MusicUser;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Queries;

public static class GetUserInfo
{
    public record Query(Guid UserId) : IRequest<Response>;

    public record Response(MusicUserInfoDto UserInfo);

    // public class Handler : IRequestHandler<Query, Response>
    // {
    //     public async Task<Response> Handle(Query request, CancellationToken cancellationToken) { }
    // }
}