﻿using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewSong
{
    public record CreateNewSongCommand(Guid UserId, SongCreationInfoDto SongCreationInfo) : IRequest;

    public class Handler : IRequestHandler<CreateNewSongCommand>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateNewSongCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var dto = request.SongCreationInfo;
            
            var genre = await _context.SongGenres.FindAsync(dto.GenreId);
            if (genre is null)
                throw new EntityNotFoundException(ExceptionMessages.GenreCannotBeFound);

            var song = new Domain.Song
            (
                dto.Name,
                genre, user,
                dto.SongContentUri,
                dto.CoverUri
            );
            
            user.MediaLibrary.AddSong(song);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}