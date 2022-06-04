using AutoMapper;
using DS.Application.DTO.Playlist;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetAuthoredPlaylists
{
    public record GetAuthoredPlaylistsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> AuthoredPlaylistsInfo);

    public class Handler : IRequestHandler<GetAuthoredPlaylistsQuery, Response>
    {
        private MusicDbContext _context;
        private IMapper _mapper;
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetAuthoredPlaylistsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            return new Response(_mapper.Map<IReadOnlyCollection<PlaylistInfoDto>>(user.MediaLibrary.AuthoredPlaylists));
        }
    }
}