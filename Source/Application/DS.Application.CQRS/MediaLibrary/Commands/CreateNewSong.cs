﻿using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess;
using DS.DataAccess.Context;
using DS.Domain;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewSong
{
    public record CreateNewSongCommand(Guid UserId, SongCreationInfoDto SongCreationInfo) : IRequest;

    public class Handler : IRequestHandler<CreateNewSongCommand>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;
        
        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Unit> Handle(CreateNewSongCommand request, CancellationToken cancellationToken)
        {
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            SongCreationInfoDto? dto = request.SongCreationInfo;
            
            SongGenre? genre = await _context.SongGenres.FindAsync(dto.GenreId);
            if (genre is null)
                throw new EntityNotFoundException(ExceptionMessages.GenreCannotBeFound);

            var song = new Domain.Song
            (
                dto.Name,
                genre, user,
                _storage.GenerateUri(),
                _storage.GenerateUri()
            );
            
            user.MediaLibrary.AddSong(song);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}