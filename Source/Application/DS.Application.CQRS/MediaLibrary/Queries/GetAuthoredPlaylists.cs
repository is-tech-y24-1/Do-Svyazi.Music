using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetAuthoredPlaylists
{
    public record GetAuthoredPlaylistsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> AuthoredPlaylistsInfo);

    public class Handler : IRequestHandler<GetAuthoredPlaylistsQuery, Response>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetAuthoredPlaylistsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException($"User {request.UserId} does not exist");

            var authoredPlaylists = new List<PlaylistInfoDto>(user.MediaLibrary.AuthoredPlaylists.Count);
            foreach (var playlist in user.MediaLibrary.AuthoredPlaylists)
            {
                var songs = GetSongDtos(playlist);
                    
                var playlistAuthorDto = new MusicUserInfoDto(user.Id, user.Name);
                var playlistDto = new PlaylistInfoDto(playlist.Name, songs, playlistAuthorDto);
                    
                authoredPlaylists.Add(playlistDto);
            }

            if (!authoredPlaylists.Any())
                throw new DoSvyaziMusicException($"User {request.UserId} does not have any authored playlists");
           
            return new Response(authoredPlaylists.AsReadOnly());
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
                    song.ContentUri
                );
                songs.Add(songDto);
            }

            return songs;
        }
    }
}