using System.Buffers;
using System.Text;
using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess;
using DS.DataAccess.Context;
using DS.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class CreateNewSong
{
    public record CreateNewSongCommand(Guid UserId, SongCreationInfoDto SongCreationInfo) : IRequest;

    public class Handler : IRequestHandler<CreateNewSongCommand>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;

        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Unit> Handle(CreateNewSongCommand request, CancellationToken cancellationToken)
        {
            Domain.MusicUser? user = _context.MusicUsers
                .Include(i => i.MediaLibrary)
                .FirstOrDefault(user => user.Id == request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            SongCreationInfoDto? dto = request.SongCreationInfo;

            SongGenre? genre = await _context.SongGenres.FindAsync(dto.GenreId);
            if (genre is null)
            {
                _context.SongGenres.Add(new SongGenre("Punk rock"));
                await _context.SaveChangesAsync(cancellationToken);
            }
            //throw new EntityNotFoundException(ExceptionMessages.GenreCannotBeFound);

            string? coverUri = null;
            // Force unwrapping is ok here because if cover is null
            // we wont get inside this condition
            if (Helpers.Helpers.ShouldGenerateUri(dto.Cover))
                coverUri = _storage.GenerateUri(dto.Cover!.Name);

            var song = new Domain.Song
            (
                dto.Name,
                genre, user,
                _storage.GenerateUri(dto.Song.Name),
                coverUri
            );

            user.MediaLibrary.AddSong(song);
            _context.MediaLibraries.Update(user.MediaLibrary);
            await _context.SaveChangesAsync(cancellationToken);

            await Helpers.Helpers.UploadFile
            (
                dto.Song.OpenReadStream(),
                song.ContentUri,
                _storage
            );

            if (dto.Cover is null || dto.Cover.Length == 0)
                return Unit.Value;

            await Helpers.Helpers.UploadFile
            (
                dto.Cover.OpenReadStream(),
                song.CoverUri,
                _storage
            );

            return Unit.Value;
        }
    }
}