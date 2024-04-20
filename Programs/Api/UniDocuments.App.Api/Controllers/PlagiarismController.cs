using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Plagiarism;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Requests;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlagiarismController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlagiarismController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Match")]
    public async Task<IActionResult> Match(MatchTextsRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryMatchTexts(request);
        var result = await _mediator.Send(query, cancellationToken);
        return new JsonResult(result);
    }

    [HttpPost("Compare")]
    public async Task<IActionResult> Compare(CompareTextsRequest request, CancellationToken cancellationToken)
    {
        var query = new QueryCompareTexts(request);
        var result = await _mediator.Send(query, cancellationToken);
        return new JsonResult(result);
    }

    [HttpGet("SearchPlagiarism")]
    public async Task<IActionResult> SearchPlagiarism(Guid documentId, int topN, CancellationToken cancellationToken)
    {
        var request = new QuerySearchPlagiarism(documentId, topN);
        var result = await _mediator.Send(request, cancellationToken);
        return new JsonResult(result);
    }
}