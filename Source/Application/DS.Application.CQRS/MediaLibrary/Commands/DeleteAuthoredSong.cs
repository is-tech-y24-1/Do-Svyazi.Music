﻿using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            Domain.Song? song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            if (song.Author.Id != request.UserId)
                throw new UnauthorizedAccessException(ExceptionMessages.SongAccessForbidden);
            
            DbSet<Domain.MusicUser>? users = _context.MusicUsers;
            foreach (Domain.MusicUser? user in users)
            {
                if (user.MediaLibrary.Songs.Contains(song)) 
                    user.MediaLibrary.DeleteSong(song);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            
            var mediaLibrary = await _context.MediaLibraries.FindAsync(request.UserId);
            if (mediaLibrary is null)
                throw new EntityNotFoundException("Something went wrong");
            
            await _context.Entry(mediaLibrary).Collection("_songs").LoadAsync(cancellationToken);
            _context.Remove(song);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}