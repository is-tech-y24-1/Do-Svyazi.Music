using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetAuthoredPlaylists
{
    public record GetAuthoredPlaylistsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> AuthoredPlaylistsInfo);

    // public class Handler : IRequestHandler<GetAuthoredPlaylistsQuery, Response>
    // {
    //     public async Task<Response> Handle(GetAuthoredPlaylistsQuery request, CancellationToken cancellationToken) { }
    // }
}