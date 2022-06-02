using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetPlaylists
{
    public record GetPlaylistsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> PlaylistsInfo);

    public class Handler : IRequestHandler<GetPlaylistsQuery, Response>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            var playlistsDtos = new List<PlaylistInfoDto>(user.MediaLibrary.Playlists.Count);
            foreach (var playlist in user.MediaLibrary.Playlists)
            {
                var songsDtos = GetSongDtos(playlist);
                
                var authorDto = new MusicUserInfoDto(playlist.Author.Id, playlist.Author.Name);
                var playlistDto = new PlaylistInfoDto(playlist.Name, songsDtos, authorDto);
                
                playlistsDtos.Add(playlistDto);
            }

            return new Response(playlistsDtos.AsReadOnly());
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