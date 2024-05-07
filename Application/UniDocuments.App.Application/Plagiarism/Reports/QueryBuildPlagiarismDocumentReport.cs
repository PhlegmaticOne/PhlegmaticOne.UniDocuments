using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Plagiarism.Reports.Base;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Reports;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Plagiarism.Reports;

public class QueryBuildPlagiarismDocumentReport : QueryBuildPlagiarismReport
{
    public Stream FileStream { get; set; } = null!;
}

public class QueryBuildPlagiarismDocumentReportHandler : 
    IOperationResultQueryHandler<QueryBuildPlagiarismDocumentReport, PlagiarismReport>
{
    private const string ErrorMessage = "BuildPlagiarismDocumentReport.InternalError";
    
    private readonly IPlagiarismReportProvider _reportProvider;
    private readonly IStreamContentReader _contentReader;
    private readonly ILogger<QueryBuildPlagiarismDocumentReportHandler> _logger;

    public QueryBuildPlagiarismDocumentReportHandler(
        IPlagiarismReportProvider reportProvider,
        IStreamContentReader contentReader,
        ILogger<QueryBuildPlagiarismDocumentReportHandler> logger)
    {
        _reportProvider = reportProvider;
        _contentReader = contentReader;
        _logger = logger;
    }
    
    public async Task<OperationResult<PlagiarismReport>> Handle(
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
            return OperationResult.Failed<PlagiarismReport>(ErrorMessage, e.Message);
        }
    }
}