using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Song.Queries;

public static class GetSongInfo
{
    public record GetInfoQuery(Guid UserId, Guid SongId) : IRequest<Response>;

    public record Response(SongInfoDto SongInfo);

    public class Handler : IRequestHandler<GetInfoQuery, Response>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException("Song cannot be found in the database");

            var songDto = new SongInfoDto
            (
                song.Name,
                song.Genre.Name, 
                song.Author.Name,
                song.ContentUri
            );

            return new Response(songDto);
        }
    }
}