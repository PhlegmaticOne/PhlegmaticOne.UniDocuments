using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Plagiarism;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Requests;

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

    [HttpGet("SearchPlagiarismDocument")]
    public async Task<IActionResult> SearchPlagiarismDocument(
        [FromQuery] QuerySearchPlagiarismDocument request, CancellationToken cancellationToken)
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
    
    [HttpPost("Report")]
    public async Task<IActionResult> Report(
        [FromBody] QueryBuildPlagiarismReport request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.ResponseStream, result.Result.ContentType, "test.pdf");
    }
}