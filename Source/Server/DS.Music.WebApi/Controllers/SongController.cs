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
    
    [HttpGet("{userId:guid}/{songId:guid}/content")]
    public async Task<FileStreamResult?> GetSongContent(Guid userId, Guid songId)
    {
        var songInfo = await _mediator.Send(new GetSongContent.GetSongContentQuery(userId, songId));
        return songInfo.SongContent;
    }
    
    [HttpGet("{userId:guid}/{songId:guid}/cover")]
    public async Task<FileStreamResult?> GetSongCover(Guid userId, Guid songId)
    {
        var songInfo = await _mediator.Send(new GetSongCover.GetSongCoverQuery(userId, songId));
        return songInfo.SongCover;
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
    
    [HttpPost("genres/create")]
    public async Task<IActionResult> CreateNewGenre([FromBody] string name)
    {
        await _mediator.Send(new CreateNewGenre.CreateNewGenreCommand(name));
        return Ok();
    }
    
    [HttpGet("genres/all")]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await _mediator.Send(new GetAllGenres.GetAllGenresQuery());
        return Ok(genres);
    }
}