using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class ChangePlaylistSongPosition
{
    public record ChangePositionCommand(Guid UserId, Guid PlaylistId, Guid SongId, int NewPosition) : IRequest;

    public class Handler : IRequestHandler<ChangePositionCommand>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ChangePositionCommand request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException("Song cannot be found in the database");

            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException($"Playlist {request.PlaylistId} does not exist");
            
            playlist.ChangeSongPosition(song, request.NewPosition);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}