using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Extensions;
using UniDocuments.App.Application.Comparing.Queries;
using UniDocuments.App.Application.Searching.Queries;
using UniDocuments.App.Application.Training.Commands;
using UniDocuments.App.Application.Training.Queries;
using UniDocuments.App.Application.Uploading.Commands;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Domain.Services.Similarity.Request;

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
    public async Task<IActionResult> UploadFile(IFormFile formFile)
    {
        var profileId = Guid.NewGuid();
        var activityId = Guid.NewGuid();
        var request = new CommandUploadDocument(profileId, activityId, formFile.OpenReadStream());
        var result = await _mediator.Send(request);
        return new JsonResult(result);
    }

    [HttpPost("Compare")]
    public async Task<IActionResult> Compare(UniDocumentsCompareRequest request)
    {
        var profileId = Guid.NewGuid();
        var query = new QueryCompareDocuments(profileId, request);
        var result = await _mediator.Send(query);
        return new JsonResult(result);
    }

    [HttpPost("Train")]
    public IActionResult Train()
    {
        var path = _savePathProvider.SavePath;
        var request = new CommandTrainDocumentsNeuralModel(path);
        _mediator.Send(request).Forget();
        return Ok();
    }

    [HttpPost("Load")]
    public IActionResult Load()
    {
        var path = _savePathProvider.SavePath;
        var request = new CommandLoadDocumentsNeuralModel(path);
        _mediator.Send(request).Forget();
        return Ok();
    }
    
    [HttpPost("FindSimilars")]
    public async Task<IActionResult> FindSimilars(string text)
    {
        var request = new QueryFindSimilarDocuments(text);
        var result = await _mediator.Send(request);
        return new JsonResult(result);
    }
    
    [HttpGet("SearchPlagiarism")]
    public async Task<IActionResult> SearchPlagiarism(Guid documentId, int topN)
    {
        var request = new QuerySearchPlagiarism(documentId, topN);
        var result = await _mediator.Send(request);
        return new JsonResult(result);
    }
}