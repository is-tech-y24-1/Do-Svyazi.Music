using DS.Application.DTO.Song;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetSongs
{
    public record GetSongsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SongInfoDto> SongsInfo);

    public class Handler : IRequestHandler<GetSongsQuery, Response>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetSongsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException($"User {request.UserId} does not exist");

            var songsDtos = new List<SongInfoDto>(user.MediaLibrary.Songs.Count);
            foreach (var song in user.MediaLibrary.Songs)
            {
                var songDto = new SongInfoDto
                (
                    song.Name,
                    song.Genre.Name,
                    song.Author.Name,
                    song.ContentUri
                );
                songsDtos.Add(songDto);
            }
            
            if (!songsDtos.Any())
                throw new DoSvyaziMusicException($"User {request.UserId} does not have any songs");

            return new Response(songsDtos.AsReadOnly());
        }
    }
}