using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Infrastructure.Roles;
using UniDocuments.App.Application.Documents.Training;
using UniDocuments.App.Shared.Users.Enums;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;
using UniDocuments.Text.Services.Neural.Keras.Options;

namespace UniDocuments.App.Api.Controllers.Documents;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NeuralModelController : ControllerBase
{
    private readonly IMediator _mediator;

    public NeuralModelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("TrainKeras")]
    [RequireAppRoles(AppRole.Admin)]
    public async Task<IActionResult> TrainKeras(
        [FromBody] NeuralTrainOptionsKeras trainOptions, CancellationToken cancellationToken)
    {
        var command = new CommandTrainModel(trainOptions, "keras2vec");
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpPost("TrainDoc2Vec")]
    [RequireAppRoles(AppRole.Admin)]
    public async Task<IActionResult> TrainDoc2Vec(
        [FromBody] NeuralTrainOptionsDoc2Vec trainOptions, CancellationToken cancellationToken)
    {
        var command = new CommandTrainModel(trainOptions, "doc2vec");
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("RebuildDocuments")]
    [RequireAppRoles(AppRole.Admin)]
    public async Task<IActionResult> RebuildDocuments(CancellationToken cancellationToken)
    {
        var command = new CommandRebuildDocuments();
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result); 
    }
}