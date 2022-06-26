using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DS.Application.CQRS.Song.Commands;

public class CreateNewGenre
{
    public record CreateNewGenreCommand(string Name) : IRequest;

    public class Handler : IRequestHandler<CreateNewGenreCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateNewGenreCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new DoSvyaziMusicException("Genre name can't be empty");

            if (await _context.SongGenres.Select(g => g.Name.ToLower())
                    .ContainsAsync(request.Name.ToLower(), cancellationToken))
                
                throw new DoSvyaziMusicException("Genre already exists");

            return Unit.Value;
        }
    }
}