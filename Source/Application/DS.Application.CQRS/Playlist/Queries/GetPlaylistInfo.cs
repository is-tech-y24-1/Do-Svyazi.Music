using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.Playlist.Queries;

public static class GetPlaylistInfo
{
    public record GetInfoQuery(Guid UserId, Guid PlaylistId) : IRequest<Response>;

    public record Response(PlaylistInfoDto PlaylistInfo);

    // public class Handler : IRequestHandler<GetInfoQuery, Response>
    // {
    //     public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken) { }
    // }
}