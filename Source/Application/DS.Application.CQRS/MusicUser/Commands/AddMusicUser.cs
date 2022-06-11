using DS.Application.DTO.MusicUser;
using DS.DataAccess;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Commands;

public static class AddMusicUser
{
    public record AddUserCommand(MusicUserCreationInfoDto MusicUserCreationInfo) : IRequest;

    public class Handler : IRequestHandler<AddUserCommand>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;
        
        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            MusicUserCreationInfoDto dto = request.MusicUserCreationInfo;
            var musicUser = new Domain.MusicUser(dto.Id, dto.Name, _storage.GenerateUri());
            _context.MusicUsers.Add(musicUser);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}