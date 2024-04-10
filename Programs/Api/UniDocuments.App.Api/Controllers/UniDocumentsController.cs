using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Comparing.Queries;
using UniDocuments.App.Application.Searching.Queries;
using UniDocuments.App.Application.Training.Commands;
using UniDocuments.App.Application.Uploading.Commands;
using UniDocuments.Text.Domain.Providers.Similarity.Requests;
using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UniDocumentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISavePathProvider _savePathProvider;

    public UniDocumentsController(IMediator mediator, ISavePathProvider savePathProvider)
    {
        _mediator = mediator;
        _savePathProvider = savePathProvider;
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile formFile, CancellationToken cancellationToken)
    {
        var profileId = Guid.NewGuid();
        var activityId = Guid.NewGuid();
        var request = new CommandUploadDocument(profileId, activityId, formFile.OpenReadStream());
        var result = await _mediator.Send(request, cancellationToken);
        return new JsonResult(result.GetResult());
    }

    [HttpPost("Compare")]
    public async Task<IActionResult> Compare(DocumentsSimilarityRequest request, CancellationToken cancellationToken)
    {
        var profileId = Guid.NewGuid();
        var query = new QueryCompareDocuments(profileId, request);
        var result = await _mediator.Send(query, cancellationToken);
        return new JsonResult(result.Result);
    }

    [HttpPost("Train")]
    public async Task<IActionResult> Train(CancellationToken cancellationToken)
    {
        var path = _savePathProvider.SavePath;
        var request = new CommandTrainDocumentsNeuralModel(path);
        await _mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpPost("Load")]
    public async Task<IActionResult> Load(CancellationToken cancellationToken)
    {
        var path = _savePathProvider.SavePath;
        var request = new CommandLoadDocumentsNeuralModel(path);
        await _mediator.Send(request, cancellationToken);
        return Ok();
    }
    
    [HttpGet("SearchPlagiarism")]
    public async Task<IActionResult> SearchPlagiarism(Guid documentId, int topN, CancellationToken cancellationToken)
    {
        var request = new QuerySearchPlagiarism(documentId, topN);
        var result = await _mediator.Send(request, cancellationToken);
        return new JsonResult(result);
    }
}