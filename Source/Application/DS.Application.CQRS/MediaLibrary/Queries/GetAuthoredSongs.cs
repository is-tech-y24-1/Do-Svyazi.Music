using AutoMapper;
using DS.Application.DTO.Song;
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
        private IMapper _mapper;
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetAuthoredSongsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            return new Response(_mapper.Map<IReadOnlyCollection<SongInfoDto>>(user.MediaLibrary.AuthoredSongs));
        }
    }
}