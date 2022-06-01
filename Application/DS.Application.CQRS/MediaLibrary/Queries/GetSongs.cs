using DS.Application.DTO.Song;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetSongs
{
    public record Query(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SongInfoDto> SongsInfo);

    // public class Handler : IRequestHandler<Query, Response>
    // {
    //     public async Task<Response> Handle(Query request, CancellationToken cancellationToken) { }
    // }
}