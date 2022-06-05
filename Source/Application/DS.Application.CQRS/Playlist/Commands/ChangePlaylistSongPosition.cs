using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class ChangePlaylistSongPosition
{
    public record ChangePositionCommand(Guid UserId, Guid PlaylistId, Guid SongId, int NewPosition) : IRequest;

    public class Handler : IRequestHandler<ChangePositionCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ChangePositionCommand request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);

            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);
            
            playlist.ChangeSongPosition(song, request.NewPosition);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}