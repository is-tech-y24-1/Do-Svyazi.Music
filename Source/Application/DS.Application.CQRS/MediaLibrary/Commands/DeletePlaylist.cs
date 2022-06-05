using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeletePlaylist
{
    public record DeletePlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;
    
    public class Handler : IRequestHandler<DeletePlaylistCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);

            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);
            
            user.MediaLibrary.DeletePlaylist(playlist);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}