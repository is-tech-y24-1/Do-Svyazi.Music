using DS.Application.DTO.Song;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetAuthoredSongs
{
    public record Query(Guid UserId) : IRequest<Response>;
    
    public record Response(IReadOnlyCollection<SongInfoDto> AuthoredSongsInfo);
    
    // public class Handler : IRequestHandler<Query, Response>
    // {
    //     public async Task<Response> Handle(Query request, CancellationToken cancellationToken) { }
    // }
}