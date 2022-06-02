using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetPlaylists
{
    public record GetPlaylistsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> PlaylistsInfo);

    // public class Handler : IRequestHandler<GetPlaylistsQuery, Response>
    // {
    //     public async Task<Response> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken) { }
    // }
}