using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Requests;
using UniDocuments.App.Application.Plagiarism.RawSearching;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PlagiarismController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlagiarismController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("SearchText")]
    public async Task<IActionResult> SearchText(
        [FromBody] QuerySearchPlagiarismText request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("SearchDocument")]
    public async Task<IActionResult> SearchDocument(
        [FromForm] DocumentSearchPlagiarismRequest request, CancellationToken cancellationToken)
    {
        var query = new QuerySearchPlagiarismDocument
        {
            FileStream = request.File.OpenReadStream(),
            TopCount = request.TopCount,
            ModelName = request.ModelName,
            InferEpochs = request.InferEpochs
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpGet("SearchExistingDocument")]
    public async Task<IActionResult> SearchExistingDocument(
        [FromQuery] QuerySearchPlagiarismExistingDocument request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
}