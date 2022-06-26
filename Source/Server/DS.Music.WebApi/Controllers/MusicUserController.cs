using System.Net.Mime;
using DS.Application.CQRS.MusicUser.Commands;
using DS.Application.CQRS.MusicUser.Queries;
using DS.Application.DTO.MusicUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DS.Music.WebApi.Controllers;

[Route("user")]
[ApiController]
public class MusicUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public MusicUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(nameof(CreateMusicUser))]
    public async Task<IActionResult> CreateMusicUser([FromForm] MusicUserCreationInfoDto creationInfo)
    {
        await _mediator.Send(new AddMusicUser.AddUserCommand(creationInfo));
        GetUserInfo.Response? createdUserInfo = await _mediator.Send(new GetUserInfo.GetInfoQuery(creationInfo.Id));
        return Ok(createdUserInfo);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetMusicUserInfo(Guid userId)
    {
        GetUserInfo.Response? userInfo = await _mediator.Send(new GetUserInfo.GetInfoQuery(userId));
        return Ok(userInfo);
    }

    [HttpGet("profilePicture/{userId:guid}")]
    public async Task<FileResult> GetMusicUserProfilePicture(Guid userId)
    {
        var profilePicture = await _mediator.Send(new GetUserProfilePicture.GetPictureQuery(userId));
        return profilePicture.ProfilePicture!;
    }
}