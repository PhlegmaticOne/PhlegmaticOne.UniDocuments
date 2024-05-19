﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDocuments.App.Api.Infrastructure.Roles;
using UniDocuments.App.Application.Documents.Reports;

namespace UniDocuments.App.Api.Controllers.Documents;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReportsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("BuildForDocument")]
    [RequireStudyRoles(Shared.Users.Enums.StudyRole.Teacher)]
    public async Task<IActionResult> BuildForDocument(
        [FromForm] DocumentBuildReportRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<QueryBuildPlagiarismDocumentReport>(request);
        var response = await _mediator.Send(query, cancellationToken);
        
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        var result = response.Result!;
        return File(result.ResponseStream, result.ContentType, result.Name);
    }

    [HttpGet("BuildForExistingDocument")]
    [RequireStudyRoles(Shared.Users.Enums.StudyRole.Teacher)]
    public async Task<IActionResult> BuildForExistingDocument(
        [FromQuery] QueryBuildPlagiarismExistingDocumentReport request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        var result = response.Result!;
        return File(result.ResponseStream, result.ContentType, result.Name);
    }
    
    [HttpGet("BuildForExistingDocumentDefault")]
    [RequireStudyRoles(Shared.Users.Enums.StudyRole.Teacher)]
    public async Task<IActionResult> BuildForExistingDocumentDefault(
        [FromQuery] Guid documentId, CancellationToken cancellationToken)
    {
        var request = new QueryBuildPlagiarismExistingDocumentReport
        {
            BaseMetric = "cosine",
            ModelName = "doc2vec",
            DocumentId = documentId
        };
        
        var response = await _mediator.Send(request, cancellationToken);
        
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        var result = response.Result!;
        return File(result.ResponseStream, result.ContentType, result.Name);
    }
}

public class DocumentBuildReportRequest
{
    public IFormFile File { get; set; } = null!;
    public int TopCount { get; set; }
    public int InferEpochs { get; set; }
    public string ModelName { get; set; } = null!;
    public string BaseMetric { get; set; } = null!;
}