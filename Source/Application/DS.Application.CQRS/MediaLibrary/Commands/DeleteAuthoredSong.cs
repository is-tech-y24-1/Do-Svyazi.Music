﻿using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class DeleteAuthoredSong
{
    public record DeleteAuthoredSongCommand(Guid UserId, Guid SongId) : IRequest;

    public class Handler : IRequestHandler<DeleteAuthoredSongCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteAuthoredSongCommand request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            if (song.Author.Id != request.UserId)
                throw new UnauthorizedAccessException(ExceptionMessages.SongAccessForbidden);
            
            var users = _context.MusicUsers;
            foreach (var user in users)
            {
                if (user.MediaLibrary.Songs.Contains(song)) 
                    user.MediaLibrary.DeleteSong(song);
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}