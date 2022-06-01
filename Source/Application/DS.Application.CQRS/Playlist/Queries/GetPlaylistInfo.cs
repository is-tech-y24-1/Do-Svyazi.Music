using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.Playlist.Queries;

public static class GetPlaylistInfo
{
    public record Query(Guid UserId, Guid PlaylistId) : IRequest<Response>;

    public record Response(PlaylistInfoDto PlaylistInfo);

    // public class Handler : IRequestHandler<Query, Response>
    // {
    //     public async Task<Response> Handle(Query request, CancellationToken cancellationToken) { }
    // }
}