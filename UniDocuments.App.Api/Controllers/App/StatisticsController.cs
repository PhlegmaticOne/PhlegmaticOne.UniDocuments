using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.App.Statistics;

namespace UniDocuments.App.Api.Controllers.App;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetStatistics")]
    public async Task<IActionResult> GetStatistics(CancellationToken cancellationToken)
    {
        var command = new QueryGetStatistics();
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
}