using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeletePlaylist
{
    public record DeletePlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;
    
    public class Handler : IRequestHandler<DeletePlaylistCommand>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException($"Playlist {request.PlaylistId} does not exist");

            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException($"User {request.UserId} does not exist");
            
            user.MediaLibrary.DeletePlaylist(playlist);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}