using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Training;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NeuralModelController : ControllerBase
{
    private readonly IMediator _mediator;

    public NeuralModelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("BuildVocab")]
    public async Task<IActionResult> BuildVocab(CancellationToken cancellationToken)
    {
        var request = new CommandBuildVocab();
        var result = await _mediator.Send(request, cancellationToken);

        if (result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok();
    }
    
    [HttpPost("LoadVocab")]
    public async Task<IActionResult> LoadVocab(CancellationToken cancellationToken)
    {
        var request = new CommandLoadVocab();
        var result = await _mediator.Send(request, cancellationToken);

        if (result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok();
    }
    
    [HttpPost("Train")]
    public async Task<IActionResult> Train(CancellationToken cancellationToken)
    {
        var request = new CommandTrainDocumentsNeuralModel();
        var result = await _mediator.Send(request, cancellationToken);

        if (result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok();
    }

    [HttpPost("Load")]
    public async Task<IActionResult> Load(CancellationToken cancellationToken)
    {
        var request = new CommandLoadDocumentsNeuralModel();
        var result = await _mediator.Send(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok();
    }
}