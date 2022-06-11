﻿using DS.Application.DTO.Playlist;
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
            for (int i = 1; i < dto.SongsIds.Count(); i++)
            {
                Guid songId = dto.SongsIds[i];
                
                Domain.Song? song = await _context.Songs.FindAsync(songId);
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
                _storage.GenerateUri()
            );
            
            user.MediaLibrary.AddPlaylist(playlist);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}