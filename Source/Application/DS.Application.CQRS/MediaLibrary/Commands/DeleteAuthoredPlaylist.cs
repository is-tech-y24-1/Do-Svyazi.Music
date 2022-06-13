using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeleteAuthoredPlaylist
{
    public record DeleteAuthoredPlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;
    
    
    public class Handler : IRequestHandler<DeleteAuthoredPlaylistCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteAuthoredPlaylistCommand request, CancellationToken cancellationToken)
        {
            Domain.Playlist? playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);
            
            if (playlist.Author.Id != request.UserId)
                throw new UnauthorizedAccessException(ExceptionMessages.PlaylistModificationForbidden);
            
            DbSet<Domain.MusicUser>? users = _context.MusicUsers;
            foreach (Domain.MusicUser? user in users)
            {
                if (user.MediaLibrary.Playlists.Contains(playlist)) 
                    user.MediaLibrary.DeletePlaylist(playlist);
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            var mediaLibrary = await _context.MediaLibraries.FindAsync(request.UserId);
            if (mediaLibrary is null)
                throw new EntityNotFoundException(nameof(MediaLibrary));
            
            await _context.Entry(mediaLibrary).Collection("_playlists").LoadAsync(cancellationToken);
            _context.Remove(playlist);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}