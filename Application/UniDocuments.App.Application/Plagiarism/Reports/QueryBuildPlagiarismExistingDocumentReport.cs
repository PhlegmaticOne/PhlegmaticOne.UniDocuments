using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Plagiarism.Reports.Base;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.App.Application.Plagiarism.Reports;

public class QueryBuildPlagiarismExistingDocumentReport : QueryBuildPlagiarismReport
{
    public Guid DocumentId { get; set; }
}

public class QueryBuildPlagiarismExistingDocumentReportHandler : 
    IOperationResultQueryHandler<QueryBuildPlagiarismExistingDocumentReport, ReportResponse>
{
    private const string ErrorMessage = "BuildPlagiarismExistingDocumentReport.InternalError";

    private readonly IReportProvider _reportProvider;
    private readonly IDocumentLoadingProvider _loadingProvider;
    private readonly ILogger<QueryBuildPlagiarismExistingDocumentReportHandler> _logger;

    public QueryBuildPlagiarismExistingDocumentReportHandler(
        IReportProvider reportProvider,
        IDocumentLoadingProvider loadingProvider,
        ILogger<QueryBuildPlagiarismExistingDocumentReportHandler> logger)
    {
        _reportProvider = reportProvider;
        _loadingProvider = loadingProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<ReportResponse>> Handle(
        QueryBuildPlagiarismExistingDocumentReport request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _loadingProvider.LoadAsync(request.DocumentId, true, cancellationToken);
            var plagiarismRequest = request.ToReportRequest(document);
            var report = await _reportProvider.BuildReportAsync(plagiarismRequest, cancellationToken);
            return OperationResult.Successful(report);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<ReportResponse>(ErrorMessage, e.Message);
        }
    }
}