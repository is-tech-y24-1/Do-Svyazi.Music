using DS.Application.DTO.MusicUser;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Queries;

public static class GetUserInfo
{
    public record GetInfoQuery(Guid UserId) : IRequest<Response>;

    public record Response(MusicUserInfoDto UserInfo);

    // public class Handler : IRequestHandler<GetInfoQuery, Response>
    // {
    //     public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken) { }
    // }
}