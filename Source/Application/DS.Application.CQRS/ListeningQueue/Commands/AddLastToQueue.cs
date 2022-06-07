﻿using DS.Common.Exceptions;
using DS.Common.Enums;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.ListeningQueue.Commands;

public static class AddLastToQueue
{
    public record AddLastToQueueCommand(Guid UserId, Guid SongId) : IRequest;

    public class Handler : IRequestHandler<AddLastToQueueCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(AddLastToQueueCommand request, CancellationToken cancellationToken)
        {
            Domain.Song? song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            Domain.MusicUser? musicUser = await _context.MusicUsers.FindAsync(request.UserId);
            if (musicUser is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            Domain.ListeningQueue? listeningQueue = await _context.ListeningQueues.FindAsync(musicUser.ListeningQueue.OwnerId);
            if (listeningQueue is null)
                throw new EntityNotFoundException(ExceptionMessages.ListeningQueueCannotBeFound);

            listeningQueue.AddLastToPlay(song);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}