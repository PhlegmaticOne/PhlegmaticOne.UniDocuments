using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Base;
using UniDocuments.App.Application.Documents.Loading.Commands;
using UniDocuments.App.Application.Documents.Loading.Queries;
using UniDocuments.App.Shared.Documents;

namespace UniDocuments.App.Api.Controllers.Documents;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : IdentityController
{
    private const string ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DocumentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload(
        [FromForm] DocumentUploadObject uploadObject, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CommandUploadDocument>(uploadObject, o => o.AfterMap((_, d) => d.ProfileId = ProfileId()));
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return new JsonResult(result);
    }
    
    [HttpGet("Download")]
    public async Task<IActionResult> Download(
        [FromQuery] Guid documentId, CancellationToken cancellationToken)
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