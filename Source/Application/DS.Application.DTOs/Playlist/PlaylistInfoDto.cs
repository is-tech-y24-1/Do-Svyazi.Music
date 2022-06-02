using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Song;

namespace DS.Application.DTO.Playlist;

public record PlaylistInfoDto
(
    string Name,
    IList<SongInfoDto> Songs,
    MusicUserInfoDto Author
);