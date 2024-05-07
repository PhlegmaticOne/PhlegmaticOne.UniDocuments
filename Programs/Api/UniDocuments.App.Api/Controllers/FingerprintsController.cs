using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Requests;
using UniDocuments.App.Application.Plagiarism.Fingerprinting;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class FingerprintsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FingerprintsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("GetText")]
    public async Task<IActionResult> GetText(
        [FromBody] QueryCalculateFingerprintText request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("GetDocument")]
    public async Task<IActionResult> GetDocument(IFormFile document, CancellationToken cancellationToken)
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

    [HttpGet("GetExistingDocument")]
    public async Task<IActionResult> GetExistingDocument(
        [FromQuery] QueryCalculateFingerprintExistingDocument request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
    
    [HttpPost("CompareDocumentFingerprints")]
    public async Task<IActionResult> CompareDocumentFingerprints(
        [FromForm] DocumentCompareFingerprintsRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryCompareDocumentFingerprints
        {
            Original = request.Source.OpenReadStream(),
            Suspicious = request.Suspicious.OpenReadStream()
        };
        
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
}