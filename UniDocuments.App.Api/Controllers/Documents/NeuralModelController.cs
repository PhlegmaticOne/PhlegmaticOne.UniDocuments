using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.OperationResults;
using UniDocuments.App.Application.Documents.Training;
using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.App.Api.Controllers.Documents;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
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

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
    
    [HttpPost("Train")]
    public async Task<IActionResult> Train(
        [FromBody] CommandTrainDocumentsNeuralModel command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpPost("Load")]
    public async Task<IActionResult> Load(
        [FromQuery] CommandLoadDocumentsNeuralModel command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
}