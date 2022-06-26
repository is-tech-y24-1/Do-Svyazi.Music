using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess;
using DS.DataAccess.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DS.Application.CQRS.MusicUser.Queries;

public static class GetUserProfilePicture
{
    public record GetPictureQuery(Guid UserId) : IRequest<Response>;

    public record Response(FileStreamResult? ProfilePicture);

    public class Handler : IRequestHandler<GetPictureQuery, Response>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;

        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Response> Handle(GetPictureQuery request, CancellationToken cancellationToken)
        {
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var file = await Helpers.Helpers.GetFileData(user.ProfilePictureUri, _storage);
            return new Response(file);
        }
    }
}