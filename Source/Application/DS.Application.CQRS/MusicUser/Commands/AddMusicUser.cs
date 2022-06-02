using DS.Application.DTO.MusicUser;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Commands;

public static class AddMusicUser
{
    public record AddUserCommand(MusicUserCreationInfoDto MusicUserCreationInfo) : IRequest;

    // public class Handler : IRequestHandler<AddUserCommand>
    // {
    //     public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken) { }
    // }
}