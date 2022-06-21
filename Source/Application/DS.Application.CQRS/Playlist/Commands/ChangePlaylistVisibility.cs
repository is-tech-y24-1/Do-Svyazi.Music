using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class ChangePlaylistVisibility
{
    public record ChangePlaylistVisibilityCommand(Guid UserId, Guid PlaylistId, bool CommunityVisible) : IRequest;
    
    public class Handler : IRequestHandler< ChangePlaylistVisibilityCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ChangePlaylistVisibilityCommand request, CancellationToken cancellationToken)
        {
            Domain.Playlist? playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);

            playlist.SharedForCommunity = request.CommunityVisible;
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}