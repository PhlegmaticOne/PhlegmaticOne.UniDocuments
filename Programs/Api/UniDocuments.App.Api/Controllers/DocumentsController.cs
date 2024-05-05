using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UniDocuments.App.Api.Infrastructure;
using UniDocuments.App.Application.ContentRead;
using UniDocuments.App.Application.Uploading;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class DocumentsController : ControllerBase
{
    private const string ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    
    private readonly IMediator _mediator;
    private readonly IOptions<ApplicationConfiguration> _options;

    public DocumentsController(IMediator mediator, IOptions<ApplicationConfiguration> options)
    {
        _mediator = mediator;
        _options = options;
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(IFormFile formFile, CancellationToken cancellationToken)
    {
        var config = _options.Value.TestConfiguration;
        var profileId = config.UserId;
        var activityId = config.ActivityId;
        var request = new CommandUploadDocument(profileId, activityId, formFile.OpenReadStream(), formFile.FileName);
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }

    [HttpGet("GetParagraphById")]
    public async Task<IActionResult> GetParagraphById(
        [FromQuery] QueryReadParagraphById query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpGet("GetDocumentContentByGlobalId")]
    public async Task<IActionResult> GetDocumentContentByGlobalId(
        [FromQuery] QueryReadDocumentContentByGlobalId query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpGet("GetDocumentContentById")]
    public async Task<IActionResult> GetDocumentContentById(
        [FromQuery] QueryReadDocumentContentById query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpGet("GetDocumentFileById")]
    public async Task<IActionResult> GetDocumentFileById(
        [FromQuery] QueryGetDocumentById query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.Stream!, ContentType, result.Result.Name);
    }
}