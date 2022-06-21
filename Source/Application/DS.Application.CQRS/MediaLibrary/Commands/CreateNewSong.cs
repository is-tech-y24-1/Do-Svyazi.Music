using System.Buffers;
using System.Text;
using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess;
using DS.DataAccess.Context;
using DS.Domain;
using MediatR;

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
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            SongCreationInfoDto? dto = request.SongCreationInfo;
            
            SongGenre? genre = await _context.SongGenres.FindAsync(dto.GenreId);
            if (genre is null)
                throw new EntityNotFoundException(ExceptionMessages.GenreCannotBeFound);

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
            await _context.SaveChangesAsync(cancellationToken);

            await using (var stream = dto.Song.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                byte[] data = Encoding.ASCII.GetBytes(await reader.ReadToEndAsync());
                await _storage.CreateStorageFile(song.ContentUri, data);
            }
            
            if (dto.Cover is null || dto.Cover.Length == 0)
                return Unit.Value;
            
            await using (var stream = dto.Cover.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                byte[] data = Encoding.ASCII.GetBytes(await reader.ReadToEndAsync());
                if (song.CoverUri is not null)
                    await _storage.CreateStorageFile(song.CoverUri, data);
            }
            
            return Unit.Value;
        }
    }
}