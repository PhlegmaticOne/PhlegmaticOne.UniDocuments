using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.Uploading;

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
    public async Task<IActionResult> UploadFile(IFormFile formFile, CancellationToken cancellationToken)
    {
        var profileId = Guid.NewGuid();
        var activityId = Guid.NewGuid();
        var request = new CommandUploadDocument(profileId, activityId, formFile.OpenReadStream());
        var result = await _mediator.Send(request, cancellationToken);
        return new JsonResult(result.GetResult());
    }
}