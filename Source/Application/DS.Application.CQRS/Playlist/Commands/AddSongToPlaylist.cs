using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Commands;

public static class AddSongToPlaylist
{
    public record AddPlaylistSongCommand(Guid UserId, Guid PlaylistId, Guid SongId) : IRequest;

    public class Handler : IRequestHandler<AddPlaylistSongCommand>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddPlaylistSongCommand request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException("Song cannot be found in the database");

            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException($"Playlist {request.PlaylistId} does not exist");
            
            playlist.AddSong(song);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}