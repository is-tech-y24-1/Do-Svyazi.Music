using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Song.Commands;

public static class RemoveFeaturing
{
    public record RemoveFeaturingCommand(Guid UserId, Guid SongId, Guid FeaturingUserId) : IRequest;
    
    public class Handler : IRequestHandler<RemoveFeaturingCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveFeaturingCommand request, CancellationToken cancellation)
        {
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);
            
            Domain.Song? song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            song.DeleteFeaturingUser(user);
            await _context.SaveChangesAsync(cancellation);
            
            return Unit.Value;
        }
    }
}