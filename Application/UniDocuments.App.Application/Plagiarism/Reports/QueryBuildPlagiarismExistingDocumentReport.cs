﻿using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Data;

namespace UniDocuments.App.Application.Plagiarism.Reports;

public class QueryBuildPlagiarismExistingDocumentReport : IOperationResultQuery<PlagiarismReport>
{
    public Guid DocumentId { get; set; }
    public int TopN { get; set; }
    public string ModelName { get; set; } = null!;
    public string BaseMetric { get; set; } = null!;

    public PlagiarismSearchRequest ToPlagiarismSearchRequest(UniDocument document)
    {
        return new PlagiarismSearchRequest(document, TopN, ModelName);
    }
}

public class QueryBuildPlagiarismExistingDocumentReportHandler : 
    IOperationResultQueryHandler<QueryBuildPlagiarismExistingDocumentReport, PlagiarismReport>
{
    private const string ErrorMessage = "BuildPlagiarismDocumentReport.InternalError";
    
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IPlagiarismReportDataBuilder _reportDataBuilder;
    private readonly IPlagiarismReportProvider _reportProvider;
    private readonly IDocumentLoadingProvider _loadingProvider;
    private readonly ILogger<QueryBuildPlagiarismExistingDocumentReportHandler> _logger;

    public QueryBuildPlagiarismExistingDocumentReportHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IPlagiarismReportDataBuilder reportDataBuilder,
        IPlagiarismReportProvider reportProvider,
        IDocumentLoadingProvider loadingProvider,
        ILogger<QueryBuildPlagiarismExistingDocumentReportHandler> logger)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _reportDataBuilder = reportDataBuilder;
        _reportProvider = reportProvider;
        _loadingProvider = loadingProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<PlagiarismReport>> Handle(
        QueryBuildPlagiarismExistingDocumentReport request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _loadingProvider.LoadAsync(request.DocumentId, true, cancellationToken);
            var plagiarismRequest = request.ToPlagiarismSearchRequest(document);
            var plagiarismResponse = await _plagiarismSearchProvider.SearchAsync(plagiarismRequest, cancellationToken);
            var reportDataRequest = new PlagiarismReportDataRequest(document, plagiarismResponse, request.BaseMetric);
            var reportData = await _reportDataBuilder.BuildReportDataAsync(reportDataRequest, cancellationToken);
            var report = await _reportProvider.BuildReportAsync(reportData, cancellationToken);
            return OperationResult.Successful(report);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<PlagiarismReport>(ErrorMessage, e.Message);
        }
    }
}