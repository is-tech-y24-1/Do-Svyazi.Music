using AutoMapper;
using DS.Application.DTO.Playlist;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Queries;

public static class GetPlaylistInfo
{
    public record GetInfoQuery(Guid UserId, Guid PlaylistId) : IRequest<Response>;

    public record Response(PlaylistInfoDto PlaylistInfo);

    public class Handler : IRequestHandler<GetInfoQuery, Response>
    {
        private MusicDbContext _context;
        private IMapper _mapper;
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);
            
            return new Response(_mapper.Map<PlaylistInfoDto>(playlist));
        }
    }
}