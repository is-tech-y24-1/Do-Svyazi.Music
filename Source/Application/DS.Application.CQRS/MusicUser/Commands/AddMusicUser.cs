using DS.Application.DTO.MusicUser;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Commands;

public static class AddMusicUser
{
    public record AddUserCommand(MusicUserCreationInfoDto MusicUserCreationInfo) : IRequest;

    public class Handler : IRequestHandler<AddUserCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            MusicUserCreationInfoDto? dto = request.MusicUserCreationInfo;
            var musicUser = new Domain.MusicUser(dto.Id, dto.Name, dto.ProfilePictureUri);
            _context.MusicUsers.Add(musicUser);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}