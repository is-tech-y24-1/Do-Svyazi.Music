using DS.Application.CQRS.ListeningQueue.Commands;
using DS.Application.CQRS.ListeningQueue.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DS.Music.WebApi.Controllers;

[Route("queue")]
[ApiController]
public class ListeningQueueController : ControllerBase
{
    private readonly IMediator _mediator;

    public ListeningQueueController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetQueueInfo(Guid userId)
    {
        var queueInfo = await _mediator.Send(new GetQueueInfo.GetInfoQuery(userId));
        return Ok(queueInfo);
    }
    
    [HttpPut(nameof(AddLastToQueue))]
    public async Task<IActionResult> AddLastToQueue([FromBody] AddLastToQueue.AddLastToQueueCommand command)
    {
        await _mediator.Send(command);
        var queueInfo = _mediator.Send(new GetQueueInfo.GetInfoQuery(command.UserId));
        return Ok(queueInfo);
    }
    
    [HttpPut(nameof(AddNextToQueue))]
    public async Task<IActionResult> AddNextToQueue([FromBody] AddNextToQueue.AddNextToQueueCommand command)
    {
        await _mediator.Send(command);
        var queueInfo = _mediator.Send(new GetQueueInfo.GetInfoQuery(command.UserId));
        return Ok(queueInfo);
    }
    
    [HttpPut(nameof(ChangeQueueSongPosition))]
    public async Task<IActionResult> ChangeQueueSongPosition([FromBody] ChangeQueueSongPosition.ChangeQueueSongPositionCommand command)
    {
        await _mediator.Send(command);
        var queueInfo = _mediator.Send(new GetQueueInfo.GetInfoQuery(command.UserId));
        return Ok(queueInfo);
    }
    
    [HttpPut(nameof(ClearQueue))]
    public async Task<IActionResult> ClearQueue([FromBody] ClearQueue.ClearQueueCommand command)
    {
        await _mediator.Send(command);
        var queueInfo = _mediator.Send(new GetQueueInfo.GetInfoQuery(command.UserId));
        return Ok(queueInfo);
    }
    
    [HttpPut(nameof(DeleteSongFromQueue))]
    public async Task<IActionResult> DeleteSongFromQueue([FromBody] DeleteFromQueue.DeleteFromQueueCommand command)
    {
        await _mediator.Send(command);
        var queueInfo = _mediator.Send(new GetQueueInfo.GetInfoQuery(command.UserId));
        return Ok(queueInfo);
    }
}