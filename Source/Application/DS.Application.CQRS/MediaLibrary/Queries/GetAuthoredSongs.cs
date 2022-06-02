using DS.Application.DTO.Song;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetAuthoredSongs
{
    public record GetAuthoredSongsQuery(Guid UserId) : IRequest<Response>;
    
    public record Response(IReadOnlyCollection<SongInfoDto> AuthoredSongsInfo);
    
    // public class Handler : IRequestHandler<GetAuthoredSongsQuery, Response>
    // {
    //     public async Task<Response> Handle(GetAuthoredSongsQuery request, CancellationToken cancellationToken) { }
    // }
}