using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Song.Commands;

public static class ChangeSongVisibility
{
    public record ChangeSongVisibilityCommand(Guid UserId, Guid SongId, bool CommunityVisible) : IRequest;
    
    public class Handler : IRequestHandler<ChangeSongVisibilityCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ChangeSongVisibilityCommand request, CancellationToken cancellation)
        {
            Domain.Song? song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            song.SharedForCommunity = request.CommunityVisible;
            await _context.SaveChangesAsync(cancellation);
            
            return Unit.Value;
        }
    }
}