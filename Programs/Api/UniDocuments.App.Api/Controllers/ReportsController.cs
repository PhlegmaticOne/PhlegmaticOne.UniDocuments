using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Requests;
using UniDocuments.App.Application.Plagiarism.Reports;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("BuildText")]
    public async Task<IActionResult> BuildText(
        [FromBody] QueryBuildPlagiarismTextReport request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.ResponseStream, result.Result.ContentType, result.Result.Name);
    }

    [HttpPost("BuildDocument")]
    public async Task<IActionResult> BuildDocument(
        [FromForm] DocumentBuildReportRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryBuildPlagiarismDocumentReport
        {
            FileStream = request.File.OpenReadStream(),
            ModelName = request.ModelName,
            TopCount = request.TopCount,
            BaseMetric = request.BaseMetric,
            InferEpochs = request.InferEpochs
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.ResponseStream, result.Result.ContentType, result.Result.Name);
    }

    [HttpGet("BuildExistingDocument")]
    public async Task<IActionResult> BuildExistingDocument(
        [FromQuery] QueryBuildPlagiarismExistingDocumentReport request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.ResponseStream, result.Result.ContentType, result.Result.Name);
    }
}