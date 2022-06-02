using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeleteAuthoredPlaylist
{
    public record DeleteAuthoredPlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;
    
    
    public class Handler : IRequestHandler<DeleteAuthoredPlaylistCommand>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteAuthoredPlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException($"Playlist {request.PlaylistId} does not exist");
            
            if (playlist.Author.Id != request.UserId)
                throw new UnauthorizedAccessException($"User {request.UserId} cannot delete playlist {playlist.Id}");
            
            var users = _context.MusicUsers;
            foreach (var user in users)
            {
                if (user.MediaLibrary.Playlists.Contains(playlist)) 
                    user.MediaLibrary.DeletePlaylist(playlist);
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}