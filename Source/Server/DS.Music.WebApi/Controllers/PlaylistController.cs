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
        GetPlaylistInfo.Response? playlistInfo = await _mediator.Send(new GetPlaylistInfo.GetInfoQuery(userId, playlistId));
        return Ok(playlistInfo);
    }

    [HttpPut(nameof(AddSongToPlaylist))]
    public async Task<IActionResult> AddSongToPlaylist([FromBody] AddSongToPlaylist.AddPlaylistSongCommand command)
    {
        await _mediator.Send(command);
        Task<GetPlaylistInfo.Response>? playlistInfo = _mediator.Send(new GetPlaylistInfo.GetInfoQuery(command.UserId, command.PlaylistId));
        return Ok(playlistInfo);
    }
    
    [HttpPut(nameof(ChangePlaylistSongPosition))]
    public async Task<IActionResult> ChangePlaylistSongPosition([FromBody] ChangePlaylistSongPosition.ChangePositionCommand command)
    {
        await _mediator.Send(command);
        Task<GetPlaylistInfo.Response>? playlistInfo = _mediator.Send(new GetPlaylistInfo.GetInfoQuery(command.UserId, command.PlaylistId));
        return Ok(playlistInfo);
    }
    
    [HttpPut(nameof(DeleteSongFromPlaylist))]
    public async Task<IActionResult> DeleteSongFromPlaylist([FromBody] DeleteSongFromPlaylist.DeletePlaylistSongCommand command)
    {
        await _mediator.Send(command);
        Task<GetPlaylistInfo.Response>? playlistInfo = _mediator.Send(new GetPlaylistInfo.GetInfoQuery(command.UserId, command.PlaylistId));
        return Ok(playlistInfo);
    }
    
    [HttpPut(nameof(ChangePlaylistVisibility))]
    public async Task<IActionResult> ChangePlaylistVisibility([FromBody] ChangePlaylistVisibility.ChangePlaylistVisibilityCommand command)
    {
        await _mediator.Send(command);
        Task<GetPlaylistInfo.Response>? playlistInfo = _mediator.Send(new GetPlaylistInfo.GetInfoQuery(command.UserId, command.PlaylistId));
        return Ok(playlistInfo);
    }
}