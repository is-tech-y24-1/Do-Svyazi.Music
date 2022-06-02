using DS.Application.DTO.Song;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetSongs
{
    public record GetSongsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SongInfoDto> SongsInfo);

    // public class Handler : IRequestHandler<GetSongsQuery, Response>
    // {
    //     public async Task<Response> Handle(GetSongsQuery request, CancellationToken cancellationToken) { }
    // }
}