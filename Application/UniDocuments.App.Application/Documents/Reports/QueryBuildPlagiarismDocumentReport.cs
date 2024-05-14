using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Documents.Reports.Base;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Reports;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Documents.Reports;

public class QueryBuildPlagiarismDocumentReport : QueryBuildPlagiarismReport
{
    public Stream FileStream { get; set; } = null!;
    public string FileName { get; set; } = null!;
}

public class QueryBuildPlagiarismDocumentReportHandler : 
    IOperationResultQueryHandler<QueryBuildPlagiarismDocumentReport, ReportResponse>
{
    private const string ErrorMessage = "BuildPlagiarismDocumentReport.InternalError";
    
    private readonly IReportProvider _reportProvider;
    private readonly IStreamContentReader _contentReader;
    private readonly ILogger<QueryBuildPlagiarismDocumentReportHandler> _logger;

    public QueryBuildPlagiarismDocumentReportHandler(
        IReportProvider reportProvider,
        IStreamContentReader contentReader,
        ILogger<QueryBuildPlagiarismDocumentReportHandler> logger)
    {
        _reportProvider = reportProvider;
        _contentReader = contentReader;
        _logger = logger;
    }
    
    public async Task<OperationResult<ReportResponse>> Handle(
        QueryBuildPlagiarismDocumentReport request, CancellationToken cancellationToken)
    {
        try
        {
            var content = await _contentReader.ReadAsync(request.FileStream, cancellationToken);
            var document = UniDocument.FromContent(content);
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