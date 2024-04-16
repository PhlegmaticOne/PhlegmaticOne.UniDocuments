using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Comparing.Queries;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TextComparingController
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
}