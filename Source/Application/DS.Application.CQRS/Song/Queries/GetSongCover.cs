using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess;
using DS.DataAccess.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DS.Application.CQRS.Song.Queries;
public static class GetSongCover
{
    public record GetSongCoverQuery(Guid UserId, Guid SongId) : IRequest<Response>;

    public record Response(FileStreamResult? SongCover);

    public class Handler : IRequestHandler<GetSongCover.GetSongCoverQuery, Response>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;
        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Response> Handle(GetSongCover.GetSongCoverQuery request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            var file = await Helpers.Helpers.GetFileData(song.CoverUri, _storage);
            return new Response(file);
        }
    }
}