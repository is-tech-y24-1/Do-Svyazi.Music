using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.Song.Queries;

public static class GetSongInfo
{
    public record GetInfoQuery(Guid UserId, Guid SongId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> AuthoredPlaylistsInfo);

    // public class Handler : IRequestHandler<GetInfoQuery, Response>
    // {
    //     public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken) { }
    // }
}