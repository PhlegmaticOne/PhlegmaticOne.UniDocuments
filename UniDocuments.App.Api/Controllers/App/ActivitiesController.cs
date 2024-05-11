using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Base;
using UniDocuments.App.Application.App.Activities.Commands;
using UniDocuments.App.Application.App.Activities.Queries;
using UniDocuments.App.Shared.Activities.Create;
using UniDocuments.App.Shared.Shared;

namespace UniDocuments.App.Api.Controllers.App;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ActivitiesController : IdentityController
{
    private readonly IMediator _mediator;

    public ActivitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetForTeacher")]
    public async Task<IActionResult> GetForTeacher([FromQuery] PagedListData data, CancellationToken cancellationToken)
    {
        var query = new QueryGetActivitiesTeacher(ProfileId(), data);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(
        [FromBody] ActivityCreateObject data, CancellationToken cancellationToken)
    {
        var query = new CommandCreateActivity(ProfileId(), data);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}