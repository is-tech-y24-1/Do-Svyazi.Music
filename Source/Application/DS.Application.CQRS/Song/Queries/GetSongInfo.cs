using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.Song.Queries;

public static class GetSongInfo
{
    public record Query(Guid UserId, Guid SongId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> AuthoredPlaylistsInfo);

    // public class Handler : IRequestHandler<Query, Response>
    // {
    //     public async Task<Response> Handle(Query request, CancellationToken cancellationToken) { }
    // }
}