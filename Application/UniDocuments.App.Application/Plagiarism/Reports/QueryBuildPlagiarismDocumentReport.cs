using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Data;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.App.Application.Plagiarism.Reports;

public class QueryBuildPlagiarismDocumentReport : IOperationResultQuery<PlagiarismReport>
{
    public Stream FileStream { get; set; } = null!;
    public int TopN { get; set; }
    public string ModelName { get; set; } = null!;
    public string BaseMetric { get; set; } = null!;

    public PlagiarismSearchRequest ToPlagiarismSearchRequest(UniDocument document)
    {
        return new PlagiarismSearchRequest(document, TopN, ModelName);
    }
}

public class QueryBuildPlagiarismDocumentReportHandler : 
    IOperationResultQueryHandler<QueryBuildPlagiarismDocumentReport, PlagiarismReport>
{
    private const string ErrorMessage = "BuildPlagiarismDocumentReport.InternalError";
    
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IPlagiarismReportDataBuilder _reportDataBuilder;
    private readonly IPlagiarismReportProvider _reportProvider;
    private readonly IStreamContentReader _contentReader;
    private readonly ILogger<QueryBuildPlagiarismDocumentReportHandler> _logger;

    public QueryBuildPlagiarismDocumentReportHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IPlagiarismReportDataBuilder reportDataBuilder,
        IPlagiarismReportProvider reportProvider,
        IStreamContentReader contentReader,
        ILogger<QueryBuildPlagiarismDocumentReportHandler> logger)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _reportDataBuilder = reportDataBuilder;
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