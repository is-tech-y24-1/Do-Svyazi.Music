using DS.DataAccess.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DS.Application.CQRS.Song.Queries;

public static class GetAllGenres
{
    public record GetAllGenresQuery() : IRequest<Response>;
    public record Response(List<string> Genres);

    public class Handler : IRequestHandler<GetAllGenresQuery, Response>
    {
        private readonly MusicDbContext _context;

        public Handler(MusicDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            return new Response(await _context.SongGenres.Select(g => g.Name)
                .ToListAsync(cancellationToken));
        }
    }
}