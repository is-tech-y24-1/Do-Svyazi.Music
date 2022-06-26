using System.Text;
using DS.Application.DTO.Playlist;
using DS.Common;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess;
using DS.DataAccess.Context;
using DS.Domain;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewPlaylist
{
    public record CreateNewPlaylistCommand(Guid UserId, PlaylistCreationInfoDto PlaylistCreationInfo) : IRequest;

    public class Handler : IRequestHandler<CreateNewPlaylistCommand>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;
        
        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Unit> Handle(CreateNewPlaylistCommand request, CancellationToken cancellationToken)
        {
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);
            
            PlaylistCreationInfoDto? dto = request.PlaylistCreationInfo;

            Domain.Song? firstSong = await _context.Songs.FindAsync(dto.SongsIds.First());
            if (firstSong is null)
                throw new DoSvyaziMusicException(ExceptionMessages.NoSongsInPlaylist);
            
            var songs = new PlaylistSongs(firstSong);
            for (int i = 1; i < dto.SongsIds.Count; i++)
            {
                Guid songId = dto.SongsIds[i];
                
                Domain.Song? song = await _context.Songs.FindAsync(songId);
                if (song is null)
                    throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
                
                songs.Add(song);
            }

            string? coverUri = null;
            // Force unwrapping is ok here because if cover is null
            // we wont get inside this condition
            if (Helpers.Helpers.ShouldGenerateUri(dto.Cover))
                coverUri = _storage.GenerateUri(dto.Cover!.Name);

            var playlist = new Domain.Playlist
            (
                dto.Name,
                user, 
                songs,
                dto.SharedForCommunity,
                dto.Description,
                coverUri
            );

            user.MediaLibrary.AddPlaylist(playlist);
            await _context.SaveChangesAsync(cancellationToken);
            
            if (dto.Cover is null || dto.Cover.Length == 0)
                return Unit.Value;

            await Helpers.Helpers.UploadFile
            (
                dto.Cover.OpenReadStream(),
                playlist.CoverUri,
                _storage
            );
            return Unit.Value;
        }
    }
}