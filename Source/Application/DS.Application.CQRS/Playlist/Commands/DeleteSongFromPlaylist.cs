using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class DeleteSongFromPlaylist
{
    public record DeletePlaylistSongCommand(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    public class Handler : IRequestHandler<DeletePlaylistSongCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePlaylistSongCommand request, CancellationToken cancellationToken)
        {
            Domain.Song? song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);

            Domain.Playlist? playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);
            
            playlist.DeleteSong(song);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}