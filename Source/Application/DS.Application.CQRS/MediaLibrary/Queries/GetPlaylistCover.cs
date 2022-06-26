using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess;
using DS.DataAccess.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DS.Application.CQRS.Song.Queries;
public static class GetPlaylistCover
{
    public record GetPlaylistCoverQuery(Guid UserId, Guid PlaylistId) : IRequest<Response>;

    public record Response(FileStreamResult? PlaylistCover);

    public class Handler : IRequestHandler<GetPlaylistCoverQuery, Response>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;
        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Response> Handle(GetPlaylistCoverQuery request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);
            
            var file = await Helpers.Helpers.GetFileData(playlist.CoverUri, _storage);
            return new Response(file);
        }
    }
}