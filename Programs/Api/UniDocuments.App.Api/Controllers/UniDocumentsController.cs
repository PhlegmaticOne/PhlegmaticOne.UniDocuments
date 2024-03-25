using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.OperationResults;
using UniDocuments.App.Application.Uploading.Commands;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UniDocumentsController : ControllerBase
{
    private readonly ILogger<UniDocumentsController> _logger;
    private readonly IMediator _mediator;

    public UniDocumentsController(ILogger<UniDocumentsController> logger, IMediator mediator)
    {
        _logger = logger;
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
}