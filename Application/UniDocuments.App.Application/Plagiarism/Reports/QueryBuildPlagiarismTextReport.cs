using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Plagiarism.Reports.Base;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Provider;
using UniDocuments.Text.Domain.Services.Reports;

namespace UniDocuments.App.Application.Plagiarism.Reports;

public class QueryBuildPlagiarismTextReport : QueryBuildPlagiarismReport
{
    public string Text { get; set; } = null!;
}

public class QueryBuildPlagiarismTextReportHandler : 
    IOperationResultQueryHandler<QueryBuildPlagiarismTextReport, PlagiarismReport>
{
    private const string ErrorMessage = "BuildPlagiarismTextReport.InternalError";

    private readonly IPlagiarismReportProvider _reportProvider;
    private readonly ILogger<QueryBuildPlagiarismTextReportHandler> _logger;

    public QueryBuildPlagiarismTextReportHandler(
        IPlagiarismReportProvider reportProvider,
        ILogger<QueryBuildPlagiarismTextReportHandler> logger)
    {
        _reportProvider = reportProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<PlagiarismReport>> Handle(
        QueryBuildPlagiarismTextReport request, CancellationToken cancellationToken)
    {
        try
        {
            var document = UniDocument.FromString(request.Text);
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