using AutoMapper;
using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetSongs
{
    public record GetSongsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SongInfoDto> SongsInfo);

    public class Handler : IRequestHandler<GetSongsQuery, Response>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetSongsQuery request, CancellationToken cancellationToken)
        {
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            return new Response(_mapper.Map<IReadOnlyCollection<SongInfoDto>>(user.MediaLibrary.Songs));
        }
    }
}