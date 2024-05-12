using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Requests;
using UniDocuments.App.Application.Documents.ContentRead;
using UniDocuments.App.Application.Documents.Uploading;

namespace UniDocuments.App.Api.Controllers.Documents;

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
    public async Task<IActionResult> Upload(
        [FromForm] DocumentUploadRequest uploadRequest, CancellationToken cancellationToken)
    {
        var profileId = uploadRequest.StudentId;
        var activityId = uploadRequest.ActivityId;
        var document = uploadRequest.Document;
        var request = new CommandUploadDocument(profileId, activityId, document.OpenReadStream(), document.FileName);
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
    
    [HttpGet("GetFileById")]
    public async Task<IActionResult> GetFileById([FromQuery] Guid documentId, CancellationToken cancellationToken)
    {
        var query = new QueryGetDocumentById(documentId);
        var response = await _mediator.Send(query, cancellationToken);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        var result = response.Result!;
        return File(result.Stream!, ContentType, result.Name);
    }
}