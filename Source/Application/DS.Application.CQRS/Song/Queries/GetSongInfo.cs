using AutoMapper;
using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Song.Queries;

public static class GetSongInfo
{
    public record GetInfoQuery(Guid UserId, Guid SongId) : IRequest<Response>;

    public record Response(SongInfoDto SongInfo);

    public class Handler : IRequestHandler<GetInfoQuery, Response>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken)
        {
            Domain.Song? song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException(ExceptionMessages.SongCannotBeFound);
            
            return new Response(_mapper.Map<SongInfoDto>(song));
        }
    }
}