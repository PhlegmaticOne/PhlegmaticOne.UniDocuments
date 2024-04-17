using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Comparing.Queries;
using UniDocuments.Text.Domain.Providers.Similarity.Requests;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TextComparingController : ControllerBase
{
    private readonly IMediator _mediator;

    public TextComparingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CompareByFingerprints")]
    public async Task<IActionResult> CompareByFingerprints(
        QueryCompareStringsByFingerprints query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return new JsonResult(result);
    }
    
    [HttpPost("CompareByAlgorithms")]
    public async Task<IActionResult> CompareByAlgorithms(
        TextsSimilarityRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryCompareStringsByAlgorithms(request);
        var result = await _mediator.Send(query, cancellationToken);
        return new JsonResult(result);
    }
}