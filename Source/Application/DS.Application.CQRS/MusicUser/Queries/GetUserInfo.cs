using AutoMapper;
using DS.Application.DTO.MusicUser;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Queries;

public static class GetUserInfo
{
    public record GetInfoQuery(Guid UserId) : IRequest<Response>;

    public record Response(MusicUserInfoDto UserInfo);

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
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            return new Response(_mapper.Map<MusicUserInfoDto>(user));
        }
    }
}