using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.OperationResults;
using UniDocuments.App.Api.Extensions;
using UniDocuments.App.Application.Comparing.Queries;
using UniDocuments.App.Application.Training.Commands;
using UniDocuments.App.Application.Training.Queries;
using UniDocuments.App.Application.Uploading.Commands;
using UniDocuments.Text.Application.Similarity;
using UniDocuments.Text.Domain.Algorithms;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UniDocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UniDocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("UploadFile")]
    public async Task<OperationResult> UploadFile(IFormFile formFile)
    {
        var profileId = Guid.NewGuid();
        var activityId = Guid.NewGuid();
        var request = new CommandUploadDocument(profileId, activityId, formFile.OpenReadStream());
        return await _mediator.Send(request);
    }

    [HttpPost("Compare")]
    public async Task<OperationResult<UniDocumentsCompareResult>> Compare(UniDocumentsCompareRequest request)
    {
        var profileId = Guid.NewGuid();
        var query = new QueryCompareDocuments(profileId, 
            request.ComparingDocumentId, request.OriginalDocumentId, request.Algorithms);
        return await _mediator.Send(query);
    }

    [HttpPost("Train")]
    public IActionResult Train()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Models");
        var request = new CommandTrainDocumentsNeuralModel(path);
        _mediator.Send(request).Forget();
        return Ok();
    }

    [HttpPost("Load")]
    public IActionResult Load()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Models");
        var request = new CommandLoadDocumentsNeuralModel(path);
        _mediator.Send(request).Forget();
        return Ok();
    }
    
    [HttpPost("FindSimilars")]
    public async Task<IActionResult> FindSimilars(string text)
    {
        var request = new QueryFindSimilarDocuments(text);
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}