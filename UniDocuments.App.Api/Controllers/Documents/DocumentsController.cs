using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Controllers.Base;
using UniDocuments.App.Api.Infrastructure.Roles;
using UniDocuments.App.Application.Documents.Data;
using UniDocuments.App.Application.Documents.Loading.Commands;
using UniDocuments.App.Application.Documents.Loading.Queries;
using UniDocuments.App.Shared.Documents;
using UniDocuments.App.Shared.Users.Enums;

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
    [RequireStudyRoles(Shared.Users.Enums.StudyRole.Student)]
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
        return File(result.ToStream(), ContentType, result.Name);
    }
    
    [HttpGet("GetGlobalData")]
    [RequireAppRoles(AppRole.Admin)]
    public async Task<IActionResult> GetGlobalData(CancellationToken cancellationToken)
    {
        var query = new QueryGetGlobalData();
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}