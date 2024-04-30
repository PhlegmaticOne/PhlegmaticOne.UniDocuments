using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Application.ContentRead;
using UniDocuments.App.Application.Uploading;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UniDocumentsController : ControllerBase
{
    private const string ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    
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
        var request = new CommandUploadDocument(profileId, activityId, formFile.OpenReadStream(), formFile.FileName);
        var result = await _mediator.Send(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return new JsonResult(result);
    }

    [HttpGet("GetParagraphById")]
    public async Task<IActionResult> GetParagraphById(int paragraphId, CancellationToken cancellationToken)
    {
        var query = new QueryReadParagraphById(paragraphId);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpGet("GetDocumentContentByGlobalId")]
    public async Task<IActionResult> GetDocumentContentByGlobalId(int documentId, CancellationToken cancellationToken)
    {
        var query = new QueryReadDocumentContentByGlobalId(documentId);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpGet("GetDocumentContentById")]
    public async Task<IActionResult> GetDocumentContentById(Guid documentId, CancellationToken cancellationToken)
    {
        var query = new QueryReadDocumentContentById(documentId);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpGet("GetDocumentFileById")]
    public async Task<IActionResult> GetDocumentFileById(Guid documentId, CancellationToken cancellationToken)
    {
        var query = new QueryGetDocumentById(documentId);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return File(result.Result!.Stream!, ContentType, result.Result.Name);
    }
}