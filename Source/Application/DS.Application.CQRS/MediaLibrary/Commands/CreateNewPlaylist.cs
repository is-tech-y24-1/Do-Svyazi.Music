using DS.Application.DTO.Playlist;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using DS.Domain;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewPlaylist
{
    public record CreateNewPlaylistCommand(Guid UserId, PlaylistCreationInfoDto PlaylistCreationInfo) : IRequest;

    public class Handler : IRequestHandler<CreateNewPlaylistCommand>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateNewPlaylistCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);
            
            var dto = request.PlaylistCreationInfo;

            var firstSong = await _context.Songs.FindAsync(dto.SongsIds.First());
            if (firstSong is null)
                throw new DoSvyaziMusicException(ExceptionMessages.NoSongsInPlaylist);
            
            var songs = new PlaylistSongs(firstSong);
            for (var i = 1; i < dto.SongsIds.Count(); i++)
            {
                var songId = dto.SongsIds[i];
                
                var song = await _context.Songs.FindAsync(songId);
                if (song is null)
                    throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
                
                songs.Add(song);
            }
            
            var playlist = new Domain.Playlist
            (
                dto.Name,
                user, songs,
                dto.SharedForCommunity,
                dto.Description,
                dto.CoverUri
            );
            
            user.MediaLibrary.AddPlaylist(playlist);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}