using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(IFormFile formFile, CancellationToken cancellationToken)
    {
        var profileId = Guid.NewGuid();
        var activityId = Guid.NewGuid();
        var request = new CommandUploadDocument(profileId, activityId, formFile.OpenReadStream(), formFile.FileName);
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
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