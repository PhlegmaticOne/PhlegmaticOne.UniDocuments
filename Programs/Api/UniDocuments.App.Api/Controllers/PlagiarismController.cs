using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Requests;
using UniDocuments.App.Application.Plagiarism.Fingerprinting;
using UniDocuments.App.Application.Plagiarism.RawSearching;
using UniDocuments.App.Application.Plagiarism.Reports;

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

    [HttpPost("GetFingerprintText")]
    public async Task<IActionResult> GetFingerprintText(
        [FromBody] QueryCalculateFingerprintText request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("GetFingerprintDocument")]
    public async Task<IActionResult> GetFingerprintDocument(IFormFile document, CancellationToken cancellationToken)
    {
        var request = new QueryCalculateFingerprintDocument
        {
            DocumentStream = document.OpenReadStream()
        };
        
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpGet("GetFingerprintExistingDocument")]
    public async Task<IActionResult> GetFingerprintExistingDocument(
        [FromQuery] QueryCalculateFingerprintExistingDocument request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("SearchPlagiarismText")]
    public async Task<IActionResult> SearchPlagiarismText(
        [FromBody] QuerySearchPlagiarismText request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("SearchPlagiarismDocument")]
    public async Task<IActionResult> SearchPlagiarismDocument(
        [FromForm] DocumentSearchPlagiarismRequest request, CancellationToken cancellationToken)
    {
        var query = new QuerySearchPlagiarismDocument(request.File.OpenReadStream())
        {
            TopCount = request.TopCount,
            ModelName = request.ModelName
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpGet("SearchPlagiarismExistingDocument")]
    public async Task<IActionResult> SearchPlagiarismExistingDocument(
        [FromQuery] QuerySearchPlagiarismExistingDocument request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("BuildReportText")]
    public async Task<IActionResult> BuildReportText(
        [FromBody] QueryBuildPlagiarismTextReport request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.ResponseStream, result.Result.ContentType, result.Result.Name);
    }

    [HttpPost("BuildReportDocument")]
    public async Task<IActionResult> BuildReportDocument(
        [FromForm] DocumentBuildReportRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryBuildPlagiarismDocumentReport
        {
            FileStream = request.File.OpenReadStream(),
            ModelName = request.ModelName,
            TopCount = request.TopCount,
            BaseMetric = request.BaseMetric
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.ResponseStream, result.Result.ContentType, result.Result.Name);
    }

    [HttpGet("BuildReportExistingDocument")]
    public async Task<IActionResult> BuildReportExistingDocument(
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