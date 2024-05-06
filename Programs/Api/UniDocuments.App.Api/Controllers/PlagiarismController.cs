using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Plagiarism.Fingerprinting;
using UniDocuments.App.Application.Plagiarism.RawSearching;
using UniDocuments.App.Application.Plagiarism.Reports;
using UniDocuments.App.Application.Plagiarism.Text;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Requests;

namespace UniDocuments.App.Api.Controllers;

public class DocumentSearchPlagiarismRequest
{
    public IFormFile File { get; set; } = null!;
    public int TopN { get; set; }
    public string ModelName { get; set; } = null!;
}

public class DocumentBuildReportRequest
{
    public IFormFile File { get; set; } = null!;
    public int TopN { get; set; } = 3;
    public string ModelName { get; set; } = "doc2vec";
    public string BaseMetric { get; set; } = "cosine";
}

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

    [HttpPost("Preprocess")]
    public async Task<IActionResult> Preprocess(
        [FromBody] QueryPreprocessText request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
    
    [HttpPost("Match")]
    public async Task<IActionResult> Match(
        [FromBody] MatchTextsRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryMatchTexts(request);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("Compare")]
    public async Task<IActionResult> Compare(
        [FromBody] CompareTextsRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryCompareTexts(request);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
    
    [HttpGet("CalculateFingerprintDocument")]
    public async Task<IActionResult> CalculateFingerprintDocument(
        [FromQuery] QueryCalculateFingerprintDocument request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
    
    [HttpPost("CalculateFingerprintText")]
    public async Task<IActionResult> CalculateFingerprintText(
        [FromBody] QueryCalculateFingerprintText request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
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
    
    [HttpPost("SearchPlagiarismDocument")]
    public async Task<IActionResult> SearchPlagiarismDocument(
        [FromForm] DocumentSearchPlagiarismRequest request, CancellationToken cancellationToken)
    {
        var query = new QuerySearchPlagiarismDocument(request.File.OpenReadStream(), request.TopN, request.ModelName);
        var result = await _mediator.Send(query, cancellationToken);
        
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
    
    [HttpPost("BuildReportExistingDocument")]
    public async Task<IActionResult> BuildReportExistingDocument(
        [FromBody] QueryBuildPlagiarismExistingDocumentReport request, CancellationToken cancellationToken)
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
            TopN = request.TopN,
            BaseMetric = request.BaseMetric
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.ResponseStream, result.Result.ContentType, result.Result.Name);
    }
}