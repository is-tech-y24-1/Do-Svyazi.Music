using DS.Application.CQRS.Song.Commands;
using DS.Application.CQRS.Song.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DS.Music.WebApi.Controllers;

[Route("songs")]
[ApiController]
public class SongController : ControllerBase
{
    private readonly IMediator _mediator;

    public SongController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId:guid}/{songId:guid}")]
    public async Task<IActionResult> GetSongInfo(Guid userId, Guid songId)
    {
        GetSongInfo.Response? songInfo = await _mediator.Send(new GetSongInfo.GetInfoQuery(userId, songId));
        return Ok(songInfo);
    }
    
    [HttpPut(nameof(AddFeaturingUser))]
    public async Task<IActionResult> AddFeaturingUser([FromBody] AddFeaturing.AddFeaturingCommand command)
    {
        await _mediator.Send(command);
        Task<GetSongInfo.Response>? songInfo = _mediator.Send(new GetSongInfo.GetInfoQuery(command.UserId, command.SongId));
        return Ok(songInfo);
    }
    
    
    [HttpPut(nameof(RemoveFeaturingUser))]
    public async Task<IActionResult> RemoveFeaturingUser([FromBody] RemoveFeaturing.RemoveFeaturingCommand command)
    {
        await _mediator.Send(command);
        Task<GetSongInfo.Response>? songInfo = _mediator.Send(new GetSongInfo.GetInfoQuery(command.UserId, command.SongId));
        return Ok(songInfo);
    }
    
    [HttpPut(nameof(ChangeSongVisibility))]
    public async Task<IActionResult> ChangeSongVisibility([FromBody] ChangeSongVisibility.ChangeSongVisibilityCommand command)
    {
        await _mediator.Send(command);
        Task<GetSongInfo.Response>? songInfo = _mediator.Send(new GetSongInfo.GetInfoQuery(command.UserId, command.SongId));
        return Ok(songInfo);
    }
}