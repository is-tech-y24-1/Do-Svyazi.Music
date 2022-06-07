using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class AddSongToPlaylist
{
    public record AddPlaylistSongCommand(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    public class Handler : IRequestHandler<AddPlaylistSongCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddPlaylistSongCommand request, CancellationToken cancellationToken)
        {
            Domain.Song? song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);

            Domain.Playlist? playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);
            
            playlist.AddSong(song);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}