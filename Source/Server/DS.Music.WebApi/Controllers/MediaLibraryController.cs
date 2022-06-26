using DS.Application.CQRS.MediaLibrary.Commands;
using DS.Application.CQRS.MediaLibrary.Queries;
using DS.Application.CQRS.Song.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DS.Music.WebApi.Controllers;

[Route("library")]
[ApiController]
public class MediaLibraryController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediaLibraryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{userId:guid}/authored/playlists")]
    public async Task<IActionResult> GetAuthoredPlaylists(Guid userId)
    {
        GetAuthoredPlaylists.Response playlistsInfo = await _mediator.Send(new GetAuthoredPlaylists.GetAuthoredPlaylistsQuery(userId));
        return Ok(playlistsInfo);
    }
    
    [HttpGet("{userId:guid}/authored/songs")]
    public async Task<IActionResult> GetAuthoredSongs(Guid userId)
    {
        GetAuthoredSongs.Response songsInfo = await _mediator.Send(new GetAuthoredSongs.GetAuthoredSongsQuery(userId));
        return Ok(songsInfo);
    }
    
    [HttpGet("{userId:guid}/added/playlists")]
    public async Task<IActionResult> GetAddedPlaylists(Guid userId)
    {
        GetPlaylists.Response playlistsInfo = await _mediator.Send(new GetPlaylists.GetPlaylistsQuery(userId));
        return Ok(playlistsInfo);
    }
    
    [HttpGet("{userId:guid}/{playlistId:guid}/cover")]
    public async Task<FileStreamResult?> GetPlaylistCover(Guid userId, Guid playlistId)
    {
        var cover = await _mediator.Send(new GetPlaylistCover.GetPlaylistCoverQuery(userId, playlistId));
        return cover.PlaylistCover;
    }
    
    [HttpGet("{userId:guid}/added/songs")]
    public async Task<IActionResult> GetAddedSongsInfo(Guid userId)
    {
        GetSongs.Response songsInfo = await _mediator.Send(new GetSongs.GetSongsQuery(userId));
        return Ok(songsInfo);
    }
    
    [HttpPut(nameof(AddSong))]
    public async Task<IActionResult> AddSong([FromBody] AddSong.AddSongCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpPut(nameof(AddPlaylist))]
    public async Task<IActionResult> AddPlaylist([FromBody] AddPlaylist.AddPlaylistCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpPut(nameof(DeleteSong))]
    public async Task<IActionResult> DeleteSong([FromBody] DeleteSong.DeleteSongCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpPut(nameof(DeletePlaylist))]
    public async Task<IActionResult> DeletePlaylist([FromBody] DeletePlaylist.DeletePlaylistCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpPost(nameof(CreateNewSong))]
    public async Task<IActionResult> CreateNewSong([FromForm] CreateNewSong.CreateNewSongCommand command)
    {
        await _mediator.Send(command);
        //GetAuthoredSongs.Response songsInfo = await _mediator.Send(new GetAuthoredSongs.GetAuthoredSongsQuery(command.UserId));
        return Ok();
    }
    
    [HttpPost(nameof(CreateNewPlaylist))]
    public async Task<IActionResult> CreateNewPlaylist([FromForm] CreateNewPlaylist.CreateNewPlaylistCommand command)
    {
        await _mediator.Send(command);
        GetAuthoredPlaylists.Response playlistsInfo = await _mediator.Send(new GetAuthoredPlaylists.GetAuthoredPlaylistsQuery(command.UserId));
        return Ok(playlistsInfo);
    }
    
    [HttpDelete(nameof(DeleteAuthoredSong))]
    public async Task<IActionResult> DeleteAuthoredSong([FromBody] DeleteAuthoredSong.DeleteAuthoredSongCommand command)
    {
        await _mediator.Send(command);
        GetAuthoredSongs.Response songsInfo = await _mediator.Send(new GetAuthoredSongs.GetAuthoredSongsQuery(command.UserId));
        return Ok(songsInfo);
    }
    
    [HttpDelete(nameof(DeleteAuthoredPlaylist))]
    public async Task<IActionResult> DeleteAuthoredPlaylist([FromBody] DeleteAuthoredPlaylist.DeleteAuthoredPlaylistCommand command)
    {
        await _mediator.Send(command);
        GetAuthoredPlaylists.Response playlistsInfo = await _mediator.Send(new GetAuthoredPlaylists.GetAuthoredPlaylistsQuery(command.UserId));
        return Ok(playlistsInfo);
    }
}