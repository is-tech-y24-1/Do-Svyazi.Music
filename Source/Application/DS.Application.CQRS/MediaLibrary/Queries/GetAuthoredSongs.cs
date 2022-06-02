﻿using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetAuthoredSongs
{
    public record GetAuthoredSongsQuery(Guid UserId) : IRequest<Response>;
    
    public record Response(IReadOnlyCollection<SongInfoDto> AuthoredSongsInfo);
    public class Handler : IRequestHandler<GetAuthoredSongsQuery, Response>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetAuthoredSongsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var authoredSongs = new List<SongInfoDto>(user.MediaLibrary.AuthoredSongs.Count);
            foreach (var song in user.MediaLibrary.AuthoredSongs)
            {
                var songDto = new SongInfoDto
                (
                    song.Name,
                    song.Genre.Name,
                    song.Author.Name,
                    song.ContentUri,
                    song.CoverUri
                );
                
                authoredSongs.Add(songDto);
            }

            return new Response(authoredSongs.AsReadOnly());
        }
    }
}