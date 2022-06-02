using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Playlist.Queries;

public static class GetPlaylistInfo
{
    public record GetInfoQuery(Guid UserId, Guid PlaylistId) : IRequest<Response>;

    public record Response(PlaylistInfoDto PlaylistInfo);

    public class Handler : IRequestHandler<GetInfoQuery, Response>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);

            var songsDtos = GetSongDtos(playlist);
            var authorDto = new MusicUserInfoDto(user.Id, user.Name);
            var playlistDto = new PlaylistInfoDto(playlist.Name, songsDtos, authorDto);

            return new Response(playlistDto);
        }
        
        private static List<SongInfoDto> GetSongDtos(Domain.Playlist playlist)
        {
            var songs = new List<SongInfoDto>(playlist.Songs.Count);
            foreach (var song in playlist.Songs)
            {
                var songDto = new SongInfoDto
                (
                    song.Name,
                    song.Genre.Name,
                    song.Author.Name,
                    song.ContentUri,
                    song.CoverUri
                );
                songs.Add(songDto);
            }

            return songs;
        }
    }
}