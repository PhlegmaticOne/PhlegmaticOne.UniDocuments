using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Training.Commands;
using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NeuralModelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISavePathProvider _savePathProvider;

    public NeuralModelController(IMediator mediator, ISavePathProvider savePathProvider)
    {
        _mediator = mediator;
        _savePathProvider = savePathProvider;
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
}