using DS.Application.CQRS.Playlist.Commands;
using DS.Application.CQRS.Playlist.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DS.Music.WebApi.Controllers;

[Route("playlists")]
[ApiController]
public class PlaylistController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlaylistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId:guid}/{playlistId:guid}")]
    public async Task<IActionResult> GetPlaylistInfo(Guid userId, Guid playlistId)
    {
        var playlistInfo = await _mediator.Send(new GetPlaylistInfo.GetInfoQuery(userId, playlistId));
        return Ok(playlistInfo);
    }

    [HttpPut(nameof(AddSongToPlaylist))]
    public async Task<IActionResult> AddSongToPlaylist([FromBody] AddSongToPlaylist.AddSongCommand command)
    {
        await _mediator.Send(command);
        var playlistInfo = _mediator.Send(new GetPlaylistInfo.GetInfoQuery(command.UserId, command.PlaylistId));
        return Ok(playlistInfo);
    }
    
    [HttpPut(nameof(ChangePlaylistSongPosition))]
    public async Task<IActionResult> ChangePlaylistSongPosition([FromBody] ChangePlaylistSongPosition.ChangePositionCommand command)
    {
        await _mediator.Send(command);
        var playlistInfo = _mediator.Send(new GetPlaylistInfo.GetInfoQuery(command.UserId, command.PlaylistId));
        return Ok(playlistInfo);
    }
    
    [HttpPut(nameof(DeleteSongFromPlaylist))]
    public async Task<IActionResult> DeleteSongFromPlaylist([FromBody] DeleteSongFromPlaylist.DeleteSongCommand command)
    {
        await _mediator.Send(command);
        var playlistInfo = _mediator.Send(new GetPlaylistInfo.GetInfoQuery(command.UserId, command.PlaylistId));
        return Ok(playlistInfo);
    }
}